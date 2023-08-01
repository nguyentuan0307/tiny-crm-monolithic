using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TinyCRM.API.Exceptions;
using TinyCRM.API.Models;
using TinyCRM.API.Models.User;
using TinyCRM.API.Services.IServices;
using TinyCRM.Domain.Entities.Roles;
using TinyCRM.Domain.Entities.Users;
using TinyCRM.Domain.Helper.QueryParameters;
using TinyCRM.Domain.Interfaces;

namespace TinyCRM.API.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly JwtSettings _jwtSettings;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IMapper mapper, IOptions<JwtSettings> jwtOptions, RoleManager<IdentityRole> roleManager,
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

        public async Task<IdentityResult> SignUpAsync(SignUpDto signUpDto)
        {
            _unitOfWork.BeginTransaction();
            if (!await _roleManager.RoleExistsAsync(Role.User))
            {
                await _roleManager.CreateAsync(new IdentityRole(Role.User));
            }
            var user = _mapper.Map<ApplicationUser>(signUpDto);

            var result = await _userManager.CreateAsync(user, signUpDto.Password);
            if (!result.Succeeded)
            {
                _unitOfWork.Rollback();
                throw new IdentityException(result.Errors.First());
            }

            try
            {
                await _userManager.AddToRoleAsync(user, Role.User);
                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw new BadRequestHttpException("Adding role to user failed.");
            }
            return result;
        }

        public async Task<IdentityResult> SignUpAdminAsync(SignUpDto signUpDto)
        {
            if (!await _roleManager.RoleExistsAsync(Role.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(Role.Admin));
            }

            var user = _mapper.Map<ApplicationUser>(signUpDto);

            var result = await _userManager.CreateAsync(user, signUpDto.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Role.Admin);
            }
            return result;
        }

        public async Task<UserProfileDto> UpdateProfileAsync(string id, ProfileUserUpdateDto updateDto, ClaimsPrincipal user)
        {
            if (!CanUpdateUser(user, id))
                throw new UnauthorizedHttpException("No permission to update this user.");

            var userUpdate = await _userManager.FindByIdAsync(id)
                       ?? throw new BadRequestHttpException("User not found");

            _mapper.Map(updateDto, userUpdate);
            await _userManager.UpdateAsync(userUpdate);
            return _mapper.Map<UserProfileDto>(userUpdate);
        }

        public async Task<IdentityResult> ChangePasswordAsync(string id, UserChangePasswordDto changePasswordDto, ClaimsPrincipal user)
        {
            if (!CanUpdateUser(user, id))
                throw new UnauthorizedHttpException("No permission to update this user.");
            var userUpdate = await _userManager.FindByIdAsync(id)
                       ?? throw new BadRequestHttpException("User not found");

            var result = await _userManager.ChangePasswordAsync(userUpdate, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
            return result;
        }

        private static bool CanUpdateUser(ClaimsPrincipal user, string userIdToUpdate)
        {
            var claims = user.Claims.ToList();

            var loggedInUserId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var isAdmin = claims.Any(c => c is { Type: ClaimTypes.Role, Value: "Admin" or "SuperAdmin" });

            var isSelf = loggedInUserId == userIdToUpdate;

            return isAdmin || isSelf;
        }

        public async Task<string> SignInAsync(SignInDto signInDto)
        {
            var user = await _userManager.FindByEmailAsync(signInDto.Email)
                       ?? throw new BadRequestHttpException("User not found");

            var result = await _signInManager.PasswordSignInAsync(signInDto.Email, signInDto.Password, false, true);
            if (!result.Succeeded)
            {
                throw new BadRequestHttpException("These credentials do not match our records.");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var token = GenerateToken(user.Id, user.Email!, roles);
            return token;
        }

        public async Task<UserProfileDto> GetProfileAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id)
                ?? throw new BadRequestHttpException("User not found");
            var profile = _mapper.Map<UserProfileDto>(user);
            return profile;
        }

        public async Task<IEnumerable<UserProfileDto>> GetProfileUsersAsync(ProfileUserSearchDto search)
        {
            var sorting = ConvertSort(search);
            var userQueryParameters = new UserQueryParameters
            {
                KeyWord = search.KeyWord,
                Sorting = sorting,
                PageIndex = search.PageIndex,
                PageSize = search.PageSize
            };

            var query = _userRepository.GetUsers(userQueryParameters);

            var users = await query.ToListAsync();

            return _mapper.Map<IEnumerable<UserProfileDto>>(users);
        }

        private static string ConvertSort(ProfileUserSearchDto search)
        {
            if (search.SortFilter == null) return string.Empty;
            var sort = search.SortFilter.ToString() switch
            {
                "Name" => "Name",
                "Email" => "Email",
                _ => "Name"
            };
            sort = search.SortDirection ? $"{sort} asc" : $"{sort} desc";
            return sort;
        }

        private string GenerateToken(string id, string email, IEnumerable<string> roles)
        {
            var authClaims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, id),
                new(ClaimTypes.Email,email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            authClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.ValidIssuer,
                audience: _jwtSettings.ValidAudience,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}