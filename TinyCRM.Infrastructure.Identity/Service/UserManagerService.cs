using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TinyCRM.Application.Identity;
using TinyCRM.Application.Models.User;
using TinyCRM.Domain.Const;
using TinyCRM.Domain.Exceptions;
using TinyCRM.Domain.Helper.QueryParameters;
using TinyCRM.Infrastructure.Identity.Repository.User;
using TinyCRM.Infrastructure.Identity.Role;
using TinyCRM.Infrastructure.Identity.Users;

namespace TinyCRM.Infrastructure.Identity.Service;

public class UserManagerService : IUserManager
{
    private readonly IMapper _mapper;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserRepository _userRepository;

    public UserManagerService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
        IUserRepository userRepository, IMapper mapper, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _userRepository = userRepository;
        _mapper = mapper;
        _signInManager = signInManager;
    }

    public async Task<List<UserProfileDto>> GetUsersAsync(UserQueryParameters queryParameters)
    {
        var users = await _userRepository.GetUsersAsync(queryParameters);
        return _mapper.Map<List<UserProfileDto>>(users);
    }

    public async Task<UserProfileDto> GetUserAsync(string id)
    {
        var appUser = await FindUserById(id)
                      ?? throw new EntityNotFoundException($"User with id [{id}] not found");
        return _mapper.Map<UserProfileDto>(appUser);
    }

    public async Task<UserProfileDto> CreateUserAsync(SignUpDto signUpDto)
    {
        var user = _mapper.Map<ApplicationUser>(signUpDto);
        var result = await _userManager.CreateAsync(user, signUpDto.Password);
        if (!result.Succeeded)
            throw new InvalidUpdateException(result.Errors.First().Description);
        return _mapper.Map<UserProfileDto>(user);
    }

    public async Task AddRoleAsync(string id, string name)
    {
        var user = await FindUserById(id);
        var role = await _roleManager.FindByNameAsync(name);
        if (role == null) throw new EntityNotFoundException($"Role with name [{name}] not found");

        var result = await _userManager.AddToRoleAsync(user, name);
        if (!result.Succeeded)
        {
            var error = result.Errors.First();
            throw new InvalidUpdateException(error.Description);
        }
    }

    public async Task<UserProfileDto> UpdateUserAsync(string id, ProfileUserUpdateDto updateDto)
    {
        var userUpdate = await FindUserById(id);

        _mapper.Map(updateDto, userUpdate);
        await _userManager.UpdateAsync(userUpdate);
        return _mapper.Map<UserProfileDto>(userUpdate);
    }

    public async Task DeleteUserAsync(string id)
    {
        var appUser = await FindUserById(id);
        var deleteResult = await _userManager.DeleteAsync(appUser);
        if (!deleteResult.Succeeded)
            throw new InvalidUpdateException(deleteResult.Errors.First().Description);
    }

    public async Task ChangePasswordAsync(string id, UserChangePasswordDto changePasswordDto)
    {
        var userUpdate = await FindUserById(id);

        var result = await _userManager.ChangePasswordAsync(userUpdate, changePasswordDto.CurrentPassword,
            changePasswordDto.NewPassword);
        if (!result.Succeeded)
            throw new InvalidUpdateException(result.Errors.First().Description);
    }


    public async Task RemoveRolesUserAsync(string id, IEnumerable<string> rolesToRemove)
    {
        var appUser = await FindUserById(id);
        var removeRolesResult = await _userManager.RemoveFromRolesAsync(appUser, rolesToRemove);
        if (!removeRolesResult.Succeeded)
            throw new InvalidUpdateException(removeRolesResult.Errors.First().Description);
    }

    public async Task AddRolesUserAsync(string id, IEnumerable<string> rolesToAdd)
    {
        var appUser = await FindUserById(id);
        foreach (var roleId in rolesToAdd)
        {
            var appRole = await FindRoleById(roleId);

            ValidateUpdateRolesUser(appRole);

            var updateRoleResult = await _userManager.AddToRoleAsync(appUser, appRole.Name!);
            if (!updateRoleResult.Succeeded)
                throw new InvalidUpdateException(updateRoleResult.Errors.First().Description);
        }
    }

    public async Task<IList<string>> GetRolesAsync(string id)
    {
        var user = await FindUserById(id);
        return await _userManager.GetRolesAsync(user);
    }

    private async Task<ApplicationRole> FindRoleById(string id)
    {
        var appRole = await _roleManager.FindByIdAsync(id)
                      ?? throw new EntityNotFoundException($"Role with Id[{id}] not found");
        return appRole;
    }

    private static void ValidateUpdateRolesUser(ApplicationRole appRole)
    {
        if (appRole.Name == ConstRole.SuperAdmin)
            throw new InvalidUpdateException("Cannot add SuperAdmin role to user.");
    }

    private async Task<ApplicationUser> FindUserById(string id)
    {
        var appUser = await _userManager.FindByIdAsync(id)
                      ?? throw new EntityNotFoundException($"User with id [{id}] not found");
        return appUser;
    }
}