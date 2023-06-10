using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MyNewsApi.Dtos;
using MyNewsApi.Models;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

namespace MyNewsApi.Services
{
    public interface IUserService
    {

        Task<UserManagerResponseDto> LoginUserAsync(UserLoginDto model);

    }


    public class UserService : IUserService
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        public UserService(UserManager<AppUser> userManager, IConfiguration configuration, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
        }

        public async Task<UserManagerResponseDto> LoginUserAsync(UserLoginDto model)
        {

            if (model is null)
                throw new NullReferenceException("Reigster model is null");

            UserManagerResponseDto userManager = new();


            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
            {
                userManager.Message = "There is no user with this email address";
                userManager.IsSuccess = false;

                return userManager;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded)
            {
                userManager.Message = "Invalid Password";
                userManager.IsSuccess = false;

                return userManager; ;
            }

            var claims = new[]
            {
                new Claim("Email",model.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]!));

            var token = new JwtSecurityToken(
            issuer: _configuration["AuthSettings:Issuer"],
            audience: _configuration["AuthSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(30),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));


            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            userManager.Message = tokenAsString;
            userManager.IsSuccess = true;
            userManager.ExpireDate = token.ValidTo;

            return userManager;
        }
    }
}
