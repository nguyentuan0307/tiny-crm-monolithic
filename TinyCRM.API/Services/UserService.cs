using Microsoft.AspNetCore.Identity;
using TinyCRM.API.Models.User;
using TinyCRM.API.Services.IServices;
using TinyCRM.Domain.Entities.Users;

namespace TinyCRM.API.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<IdentityResult> SignUpAsync(SignUpDto signUpDto)
        {
            var user = new ApplicationUser
            {
                Name = signUpDto.Name,
                Email = signUpDto.Email,
                PhoneNumber = signUpDto.PhoneNumber,
            };
            var result = await _userManager.CreateAsync(user, signUpDto.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
            }
            return result;
        }
    }
}
