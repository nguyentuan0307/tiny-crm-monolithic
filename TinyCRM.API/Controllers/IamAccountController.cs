using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TinyCRM.API.Models.User;
using TinyCRM.API.Services.IServices;
using TinyCRM.Domain.Entities.Roles;

namespace TinyCRM.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class IamAccountController : Controller
    {
        private readonly IUserService _userService;

        public IamAccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Sign-Up")]
        [Authorize(Policy = Policy.AdminPolicy)]
        public async Task<IActionResult> SignUp(SignUpDto signUpDto)
        {
            if (signUpDto.Password != signUpDto.ConfirmPassword)
                return BadRequest(new { Message = "Password and ConfirmPassword do not match" });

            var result = await _userService.SignUpAsync(signUpDto);
            if (result.Succeeded)
            {
                var response = new { name = signUpDto.Name, email = signUpDto.Email, phoneNumber = signUpDto.PhoneNumber };
                return Ok(response);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("error", error.Description);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("sign-Up-Admin")]
        [Authorize(policy: Policy.SuperAdminPolicy)]
        public async Task<IActionResult> SignUpAdmin(SignUpDto signUpDto)
        {
            if (signUpDto.Password != signUpDto.ConfirmPassword)
                return BadRequest(new { Message = "Password and ConfirmPassword do not match" });

            var result = await _userService.SignUpAdminAsync(signUpDto);
            if (result.Succeeded)
            {
                var response = new { name = signUpDto.Name, email = signUpDto.Email, phoneNumber = signUpDto.PhoneNumber };
                return Ok(response);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("error", error.Description);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Sign-In")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(SignInDto signInDto)
        {
            var result = await _userService.SignInAsync(signInDto);
            if (string.IsNullOrEmpty(result))
            {
                var errorResponse = new { Message = "Invalid email or password" };
                return BadRequest(errorResponse);
            }

            var response = new { Token = result };
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfile(string id)
        {
            var user = await _userService.GetProfileAsync(id);
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetProfiles([FromQuery] ProfileUserSearchDto search)
        {
            var users = await _userService.GetProfileUsersAsync(search);
            return Ok(users);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = Policy.AccessProfilePolicy)]
        public async Task<IActionResult> UpdateProfile(string id, [FromBody] ProfileUserUpdateDto updateDto)
        {
            var userProfileDto = await _userService.UpdateProfileAsync(id, updateDto);
            return Ok(userProfileDto);
        }

        [HttpPut("{id}/change-password")]
        [Authorize(Policy = Policy.AccessProfilePolicy)]
        public async Task<IActionResult> ChangePassword(string id, [FromBody] UserChangePasswordDto changePasswordDto)
        {
            var result = await _userService.ChangePasswordAsync(id, changePasswordDto);
            if (result.Succeeded)
            {
                return Ok();
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("error", error.Description);
            }
            return BadRequest(ModelState);
        }
    }
}