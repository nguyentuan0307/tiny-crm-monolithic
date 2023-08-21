using TinyCRM.Application.Models.User;

namespace TinyCRM.Application.Service.IServices;

public interface IAuthService
{
    Task<string> SignInAsync(SignInDto signInDto);

}