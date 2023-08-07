using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TinyCRM.Application.Models.User;
using TinyCRM.Application.Service.IServices;
using TinyCRM.Domain.Entities.Roles;

namespace TinyCRM.API.Controllers;

[Authorize]
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
    [Authorize(Policy = Policy.AdminPolicy)]
    public async Task<IActionResult> SignUpAsync(SignUpDto signUpDto)
    {
        if (signUpDto.Password != signUpDto.ConfirmPassword)
            return BadRequest(new { Message = "Password and ConfirmPassword do not match" });
        var user = await _userService.SignUpAsync(signUpDto);
        return CreatedAtAction(nameof(GetProfileAsync), new { id = user.Id }, user);
    }

    [HttpPost("sign-up-admin")]
    [Authorize(policy: Policy.SuperAdminPolicy)]
    public async Task<IActionResult> SignUpAdminAsync(SignUpDto signUpDto)
    {
        if (signUpDto.Password != signUpDto.ConfirmPassword)
            return BadRequest(new { Message = "Password and ConfirmPassword do not match" });
        var user = await _userService.SignUpAsync(signUpDto);
        return CreatedAtAction(nameof(GetProfileAsync), new { id = user.Id }, user);
    }

    [HttpPost("sign-in")]
    [AllowAnonymous]
    public async Task<IActionResult> SignInAsync(SignInDto signInDto)
    {
        var result = await _userService.SignInAsync(signInDto);

        return Ok(new { Token = result });
    }

    [HttpGet("{id}")]
    [ActionName(nameof(GetProfileAsync))]
    public async Task<IActionResult> GetProfileAsync(string id)
    {
        var user = await _userService.GetProfileAsync(id);
        return Ok(user);
    }

    [HttpGet]
    public async Task<IActionResult> GetProfilesAsync([FromQuery] ProfileUserSearchDto search)
    {
        var users = await _userService.GetProfilesAsync(search);
        return Ok(users);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProfileAsync(string id, [FromBody] ProfileUserUpdateDto updateDto)
    {
        var userProfileDto = await _userService.UpdateProfileAsync(id, updateDto, User);
        return Ok(userProfileDto);
    }

    [HttpPut("{id}/change-password")]
    public async Task<IActionResult> ChangePasswordAsync(string id, [FromBody] UserChangePasswordDto changePasswordDto)
    {
        await _userService.ChangePasswordAsync(id, changePasswordDto, User);
        return NoContent();
    }
}