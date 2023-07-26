using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TinyCRM.API.Exceptions;
using TinyCRM.API.Models;
using TinyCRM.API.Models.User;
using TinyCRM.API.Services.IServices;
using TinyCRM.Domain.Entities.Roles;
using TinyCRM.Domain.Entities.Users;

namespace TinyCRM.API.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly JwtSettings _jwtSettings;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IMapper mapper, IOptions<JwtSettings> jwtOptions, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _jwtSettings = jwtOptions.Value;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> SignUpAsync(SignUpDto signUpDto)
        {
            if (!await _roleManager.RoleExistsAsync(Role.User))
            {
                await _roleManager.CreateAsync(new IdentityRole(Role.User));
            }
            var user = _mapper.Map<ApplicationUser>(signUpDto);

            var result = await _userManager.CreateAsync(user, signUpDto.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Role.User);
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

        public async Task<UserProfileDto> UpdateProfileAsync(string id, ProfileUserUpdateDto updateDto)
        {
            var user = await _userManager.FindByIdAsync(id)
                       ?? throw new BadRequestHttpException("User not found");

            _mapper.Map(updateDto, user);
            await _userManager.UpdateAsync(user);
            return _mapper.Map<UserProfileDto>(user);
        }

        public async Task<IdentityResult> ChangePasswordAsync(string id, UserChangePasswordDto changePasswordDto)
        {
            var user = await _userManager.FindByIdAsync(id)
                       ?? throw new BadRequestHttpException("User not found");

            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
            return result;
        }

        public async Task<string> SignInAsync(SignInDto signInDto)
        {
            var result = await _signInManager.PasswordSignInAsync(signInDto.Email, signInDto.Password, false, true);
            if (!result.Succeeded)
            {
                throw new BadRequestHttpException("These credentials do not match our records.");
            }

            var user = await _userManager.FindByEmailAsync(signInDto.Email)
                ?? throw new BadRequestHttpException("User not found");
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
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(search.KeyWord))
            {
                query = query.Where(GetExpression(search.KeyWord));
            }

            var sorting = ConvertSort(search);
            if (!string.IsNullOrEmpty(sorting))
            {
                query.OrderBy(sorting);
            }
            query = query.Skip(search.PageSize * (search.PageIndex - 1)).Take(search.PageSize);
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

        private static Expression<Func<ApplicationUser, bool>> GetExpression(string? keyWord)
        {
            Expression<Func<ApplicationUser, bool>> expression = p => string.IsNullOrEmpty(keyWord)
                                                                      || p.Name.Contains(keyWord)
                                                                      || p.Email!.Contains(keyWord);
            return expression;
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
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiryInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
