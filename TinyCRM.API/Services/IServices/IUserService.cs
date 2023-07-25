using Microsoft.AspNetCore.Identity;
using TinyCRM.API.Models.User;

namespace TinyCRM.API.Services.IServices
{
    public interface IUserService
    {
        Task<IdentityResult> SignUpAsync(SignUpDto signUpDto);
    }
}
