using Microsoft.AspNetCore.Mvc;
using TinyCRM.API.Models.User;
using TinyCRM.API.Services.IServices;

namespace TinyCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("SignUp")]
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
    }
}
