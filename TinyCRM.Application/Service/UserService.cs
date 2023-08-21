using System.Security.Claims;
using TinyCRM.Application.Identity;
using TinyCRM.Application.Models.User;
using TinyCRM.Application.Service.IServices;
using TinyCRM.Domain.Const;
using TinyCRM.Domain.Exceptions;
using TinyCRM.Domain.Helper.QueryParameters;
using TinyCRM.Domain.Interfaces;

namespace TinyCRM.Application.Service;

public class UserService : IUserService
{
    private readonly IUserManager _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUserManager userManager,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<UserProfileDto> SignUpAsync(SignUpDto signUpDto)
    {
        _unitOfWork.BeginTransaction();
        try
        {
            var user = await _userManager.CreateUserAsync(signUpDto);
            await _userManager.AddRoleAsync(user.Id.ToString(), ConstRole.User);
            _unitOfWork.Commit();
            return user;
        }
        catch
        {
            _unitOfWork.Rollback();
            throw;
        }
    }
    

    public async Task<UserProfileDto> GetProfileAsync(string id)
    {
        return await _userManager.GetUserAsync(id);
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
        var users = await _userManager.GetUsersAsync(userQueryParameters);
        return users;
    }

    public async Task<UserProfileDto> SignUpAdminAsync(SignUpDto signUpDto)
    {
        _unitOfWork.BeginTransaction();
        try
        {
            var user = await _userManager.CreateUserAsync(signUpDto);
            await _userManager.AddRoleAsync(user.Id.ToString(), ConstRole.Admin);
            _unitOfWork.Commit();
            return user;
        }
        catch
        {
            _unitOfWork.Rollback();
            throw;
        }
    }

    public async Task<UserProfileDto> UpdateProfileAsync(string id, ProfileUserUpdateDto updateDto,
        ClaimsPrincipal user)
    {
        if (!CanUpdateUser(user, id))
            throw new InvalidUpdateException("No permission to update this user.");

        return await _userManager.UpdateUserAsync(id, updateDto);
    }

    private static bool CanUpdateUser(ClaimsPrincipal user, string userIdToUpdate)
    {
        var claims = user.Claims.ToList();

        var loggedInUserId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        var isAdmin = claims.Any(c => c is { Type: ClaimTypes.Role, Value: ConstRole.Admin or ConstRole.SuperAdmin });

        var isSelf = loggedInUserId == userIdToUpdate;

        return isAdmin || isSelf;
    }

    public async Task ChangePasswordAsync(string id, UserChangePasswordDto changePasswordDto, ClaimsPrincipal user)
    {
        if (!CanUpdateUser(user, id))
            throw new InvalidUpdateException("No permission to update this user.");

        await _userManager.ChangePasswordAsync(id, changePasswordDto);
    }

    public async Task DeleteAsync(string id, bool isSuperAdmin = false)
    {
        var roles = await _userManager.GetRolesAsync(id);
        if (roles.Contains(ConstRole.Admin) && !isSuperAdmin)
            throw new InvalidUpdateException("Cannot delete admin user.");
        await _userManager.DeleteUserAsync(id);
    }

    public async Task UpdateRoleAsync(string id, string[] roleIds)
    {
        try
        {
            var existingRoles = await _userManager.GetRolesAsync(id);
            var rolesToAdd = roleIds.Except(existingRoles).ToList();
            var rolesToRemove = existingRoles.Except(roleIds).ToList();
            _unitOfWork.BeginTransaction();

            await _userManager.RemoveRolesUserAsync(id, rolesToRemove);
            await _userManager.AddRolesUserAsync(id, rolesToAdd);
            _unitOfWork.Commit();
        }
        catch
        {
            _unitOfWork.Rollback();
            throw;
        }
    }
}