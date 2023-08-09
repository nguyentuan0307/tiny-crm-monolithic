using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TinyCRM.Application.Models;
using TinyCRM.Application.Models.User;
using TinyCRM.Application.Service.IServices;
using TinyCRM.Domain.Const;
using TinyCRM.Domain.Exceptions;
using TinyCRM.Domain.Helper.QueryParameters;
using TinyCRM.Domain.Interfaces;
using TinyCRM.Infrastructure.Identity.Repository.User;
using TinyCRM.Infrastructure.Identity.Role;
using TinyCRM.Infrastructure.Identity.Users;

namespace TinyCRM.Infrastructure.Identity.Service;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IMapper _mapper;
    private readonly JwtSettings _jwtSettings;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        IMapper mapper, IOptions<JwtSettings> jwtOptions, RoleManager<ApplicationRole> roleManager,
        IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _jwtSettings = jwtOptions.Value;
        _roleManager = roleManager;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UserProfileDto> SignUpAsync(SignUpDto signUpDto)
    {
        _unitOfWork.BeginTransaction();
        if (!await _roleManager.RoleExistsAsync(ConstRole.User))
        {
            await _roleManager.CreateAsync(new ApplicationRole(ConstRole.User));
        }
        var user = _mapper.Map<ApplicationUser>(signUpDto);

        var result = await _userManager.CreateAsync(user, signUpDto.Password);
        if (!result.Succeeded)
        {
            _unitOfWork.Rollback();
            throw new InvalidUpdateException(result.Errors.First().Description);
        }

        try
        {
            await _userManager.AddToRoleAsync(user, ConstRole.User);
            _unitOfWork.Commit();
        }
        catch
        {
            _unitOfWork.Rollback();
            throw new InvalidUpdateException("Adding role to user failed.");
        }
        return _mapper.Map<UserProfileDto>(user);
    }

    public async Task<UserProfileDto> SignUpAdminAsync(SignUpDto signUpDto)
    {
        _unitOfWork.BeginTransaction();
        if (!await _roleManager.RoleExistsAsync(ConstRole.Admin))
        {
            await _roleManager.CreateAsync(new ApplicationRole(ConstRole.Admin));
        }

        var user = _mapper.Map<ApplicationUser>(signUpDto);
        var result = await _userManager.CreateAsync(user, signUpDto.Password);
        if (!result.Succeeded)
        {
            _unitOfWork.Rollback();
            throw new InvalidUpdateException(result.Errors.First().Description);
        }
        try
        {
            await _userManager.AddToRoleAsync(user, ConstRole.User);
            _unitOfWork.Commit();
        }
        catch
        {
            _unitOfWork.Rollback();
            throw new InvalidUpdateException("Adding role to user failed.");
        }
        return _mapper.Map<UserProfileDto>(user);
    }

    public async Task<UserProfileDto> UpdateProfileAsync(string id, ProfileUserUpdateDto updateDto, ClaimsPrincipal user)
    {
        if (!CanUpdateUser(user, id))
            throw new InvalidUpdateException("No permission to update this user.");

        var userUpdate = await _userManager.FindByIdAsync(id)
                         ?? throw new EntityNotFoundException($"User with Id[{id}] not found");

        _mapper.Map(updateDto, userUpdate);
        await _userManager.UpdateAsync(userUpdate);
        return _mapper.Map<UserProfileDto>(userUpdate);
    }

    public async Task ChangePasswordAsync(string id, UserChangePasswordDto changePasswordDto, ClaimsPrincipal user)
    {
        if (!CanUpdateUser(user, id))
            throw new InvalidUpdateException("No permission to update this user.");
        var userUpdate = await _userManager.FindByIdAsync(id)
                         ?? throw new EntityNotFoundException($"User with Id[{id}] not found");

        var result = await _userManager.ChangePasswordAsync(userUpdate, changePasswordDto.CurrentPassword,
            changePasswordDto.NewPassword);
        if (!result.Succeeded)
            throw new InvalidUpdateException(result.Errors.First().Description);
    }

    public async Task DeleteAsync(string id, bool isSuperAdmin = false)
    {
        var appUser = await _userManager.FindByIdAsync(id)
                      ?? throw new EntityNotFoundException($"User with id [{id}] not found");
        var roles = await _userManager.GetRolesAsync(appUser);
        if (roles.Contains(ConstRole.Admin) && !isSuperAdmin)
            throw new InvalidUpdateException("Cannot delete admin user.");

        var deleteResult = await _userManager.DeleteAsync(appUser);
        if (!deleteResult.Succeeded)
        {
            throw new InvalidUpdateException(deleteResult.Errors.First().Description);
        }
    }

    public async Task UpdateRoleAsync(string id, string[] roleIds)
    {
        var appUser = await _userManager.FindByIdAsync(id)
                      ?? throw new EntityNotFoundException($"User with id [{id}] not found");

        var existingRoles = await _userManager.GetRolesAsync(appUser);
        var rolesToAdd = roleIds.Except(existingRoles);
        var rolesToRemove = existingRoles.Except(roleIds);

        _unitOfWork.BeginTransaction();

        try
        {
            foreach (var roleName in rolesToRemove)
            {
                var removeRoleResult = await _userManager.RemoveFromRoleAsync(appUser, roleName);
                if (removeRoleResult.Succeeded) continue;
                _unitOfWork.Rollback();
                throw new InvalidUpdateException(removeRoleResult.Errors.First().Description);
            }

            foreach (var roleId in rolesToAdd)
            {
                var appRole = await _roleManager.FindByIdAsync(roleId)
                              ?? throw new EntityNotFoundException($"Role with id [{roleId}] not found");

                var updateRoleResult = await _userManager.AddToRoleAsync(appUser, appRole.Name!);
                if (updateRoleResult.Succeeded) continue;
                _unitOfWork.Rollback();
                throw new InvalidUpdateException(updateRoleResult.Errors.First().Description);
            }

            _unitOfWork.Commit();
        }
        catch
        {
            _unitOfWork.Rollback();
            throw new InvalidUpdateException("Updating user roles failed.");
        }
    }

    private static bool CanUpdateUser(ClaimsPrincipal user, string userIdToUpdate)
    {
        var claims = user.Claims.ToList();

        var loggedInUserId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        var isAdmin = claims.Any(c => c is { Type: ClaimTypes.Role, Value: ConstRole.Admin or ConstRole.SuperAdmin });

        var isSelf = loggedInUserId == userIdToUpdate;

        return isAdmin || isSelf;
    }

    public async Task<string> SignInAsync(SignInDto signInDto)
    {
        var user = await _userManager.FindByEmailAsync(signInDto.Email)
                   ?? throw new EntityNotFoundException($"User with Email[{signInDto.Email}] not found");

        var result = await _signInManager.PasswordSignInAsync(user, signInDto.Password, false, true);
        if (!result.Succeeded)
        {
            throw new InvalidPasswordException("Invalid password");
        }

        var roles = await _userManager.GetRolesAsync(user);

        var token = await GenerateToken(user.Id, user.Email!, roles);
        return token;
    }

    public async Task<UserProfileDto> GetProfileAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id)
                   ?? throw new EntityNotFoundException($"User with Id[{id}] not found");
        var profile = _mapper.Map<UserProfileDto>(user);
        return profile;
    }

    public async Task<List<UserProfileDto>> GetProfilesAsync(ProfileUserSearchDto search)
    {
        var userQueryParameters = new UserQueryParameters
        {
            KeyWord = search.KeyWord,
            Sorting = search.ConvertSort(),
            PageIndex = search.PageIndex,
            PageSize = search.PageSize
        };

        var users = await _userRepository.GetUsersAsync(userQueryParameters);
        return _mapper.Map<List<UserProfileDto>>(users);
    }

    private async Task<string> GenerateToken(string id, string email, IList<string> roles)
    {
        var authClaims = await GenerateAuthClaims(id, email, roles);
        var authKey = GenerateAuthKey();
        var token = GenerateJwtToken(authClaims, authKey);
        return WriteJwtToken(token);
    }

    private async Task<List<Claim>> GenerateAuthClaims(string id, string email, IList<string> roles)
    {
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, id),
            new(ClaimTypes.Email,email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        authClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        foreach (var roleName in roles)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) continue;
            var permissions = await GetPermissionsForRoleAsync(role);
            authClaims.AddRange(permissions.Select(permission => new Claim("Permission", permission)));
        }
        return authClaims;
    }

    private async Task<IEnumerable<string>> GetPermissionsForRoleAsync(ApplicationRole role)
    {
        var claims = await _roleManager.GetClaimsAsync(role);

        return (from claim in claims where claim.Type == "Permission" select claim.Value).ToList();
    }

    private SymmetricSecurityKey GenerateAuthKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
    }

    private JwtSecurityToken GenerateJwtToken(List<Claim> authClaims, SecurityKey authKey)
    {
        return new JwtSecurityToken(
            issuer: _jwtSettings.ValidIssuer,
            audience: _jwtSettings.ValidAudience,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256)
        );
    }

    private static string WriteJwtToken(SecurityToken token)
    {
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}