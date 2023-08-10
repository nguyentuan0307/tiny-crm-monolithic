using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TinyCRM.Application.Models.Permissions;
using TinyCRM.Application.Models.User;
using TinyCRM.Application.Service.IServices;
using TinyCRM.Domain.Const;
using TinyCRM.Infrastructure.Logger;

namespace TinyCRM.API.Controllers;

[ApiController]
[Route("api/iam-accounts")]
public class IamAccountController : Controller
{
    private readonly IUserService _userService;

    public IamAccountController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("sign-up")]
    [Authorize(Policy = TinyCrmPermissions.Users.Create)]
    public async Task<IActionResult> SignUpAsync(SignUpDto signUpDto)
    {
        if (signUpDto.Password != signUpDto.ConfirmPassword)
            return BadRequest(new { Message = "Password and ConfirmPassword do not match" });
        var user = await _userService.SignUpAsync(signUpDto);
        LoggerService.LogInformation($"[{DateTime.Now}]Successfully Created User: {JsonSerializer.Serialize(user)}");
        return CreatedAtAction(nameof(GetProfileAsync), new { id = user.Id }, user);
    }

    [HttpPost("sign-up-admin")]
    [Authorize(TinyCrmPermissions.Users.CreateAdmin)]
    public async Task<IActionResult> SignUpAdminAsync(SignUpDto signUpDto)
    {
        if (signUpDto.Password != signUpDto.ConfirmPassword)
            return BadRequest(new { Message = "Password and ConfirmPassword do not match" });
        var user = await _userService.SignUpAdminAsync(signUpDto);
        LoggerService.LogInformation(
            $"[{DateTime.Now}]Successfully Created User Admin: {JsonSerializer.Serialize(user)}");
        return CreatedAtAction(nameof(GetProfileAsync), new { id = user.Id }, user);
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignInAsync(SignInDto signInDto)
    {
        var result = await _userService.SignInAsync(signInDto);
        LoggerService.LogInformation(
            $"[{DateTime.Now}]Successfully Signed In User: {JsonSerializer.Serialize(result)}");
        return Ok(new { Token = result });
    }

    [HttpGet("{id}")]
    [ActionName(nameof(GetProfileAsync))]
    [Authorize(Policy = TinyCrmPermissions.Users.Read)]
    public async Task<IActionResult> GetProfileAsync(string id)
    {
        var user = await _userService.GetProfileAsync(id);
        LoggerService.LogInformation($"[{DateTime.Now}]Successfully Retrieved User: {JsonSerializer.Serialize(user)}");
        return Ok(user);
    }

    [HttpGet]
    [Authorize(Policy = TinyCrmPermissions.Users.Read)]
    public async Task<IActionResult> GetProfilesAsync([FromQuery] ProfileUserSearchDto search)
    {
        var users = await _userService.GetProfilesAsync(search);
        LoggerService.LogInformation(
            $"[{DateTime.Now}]Successfully Retrieved Users: {JsonSerializer.Serialize(users)}");
        return Ok(users);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = TinyCrmPermissions.Users.Edit)]
    public async Task<IActionResult> UpdateProfileAsync(string id, [FromBody] ProfileUserUpdateDto updateDto)
    {
        var userProfileDto = await _userService.UpdateProfileAsync(id, updateDto, User);
        LoggerService.LogInformation(
            $"[{DateTime.Now}]Successfully Updated User: {JsonSerializer.Serialize(userProfileDto)}");
        return Ok(userProfileDto);
    }

    [HttpPut("{id}/change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePasswordAsync(string id, [FromBody] UserChangePasswordDto changePasswordDto)
    {
        await _userService.ChangePasswordAsync(id, changePasswordDto, User);
        LoggerService.LogInformation($"[{DateTime.Now}]Successfully Changed Password User: {id}");
        return NoContent();
    }

    [HttpPut("{id}/update-role")]
    [Authorize(Policy = TinyCrmPermissions.Users.EditRoles)]
    public async Task<IActionResult> UpdateRoleAsync(string id, [FromBody] string[] roleIds)
    {
        await _userService.UpdateRoleAsync(id, roleIds);
        LoggerService.LogInformation($"[{DateTime.Now}]Successfully Updated Role User: {id}");
        return NoContent();
    }

    [HttpPut("{id}/delete")]
    [Authorize(Policy = TinyCrmPermissions.Users.Delete)]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        var isSuperAdmin = User.IsInRole(ConstRole.SuperAdmin);
        await _userService.DeleteAsync(id, isSuperAdmin);
        LoggerService.LogInformation($"[{DateTime.Now}]Successfully Deleted User: {id}");
        return NoContent();
    }
}