namespace BAL.Services
{
    using Pizzashop.BAL.Interfaces;
    using BCrypt.Net;
    using Microsoft.Extensions.Configuration;
    using Pizzashop.DAL.ViewModels;
    using DAL.Interfaces;
    using Microsoft.IdentityModel.Tokens;
    using System.Text;
    using System.Security.Claims;
    using System.IdentityModel.Tokens.Jwt;
    // using DAL.Models;
    using System.Net.Mail;
    using System.Net;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Identity.Client;
    using BAL.Interfaces;


    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;


        public AuthService(IUserRepository userRepository, IConfiguration configuration,IEmailService emailService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<string> AuthenticateUserAsync(Loginviewmodel model)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(model.Email);

            if (existingUser == null)
            {
                throw new Exception("User does not exists");
            }

            if (!BCrypt.Verify(model.Password, existingUser.Password))
            {
                throw new Exception("Invalid password.");
            }

            if (existingUser.UserroleNavigation == null)
            {
                throw new Exception("User role not found.");
            }

            return GenerateJwtToken(existingUser.Email, existingUser.UserroleNavigation.Rolename);
        }

        private string GenerateJwtToken(string email, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<bool> SendMail(String email)
        {
            var existingUser = await _userRepository.GetUserByEmailWithRole(email);

            if (existingUser == null)
            {
                return false;
            }

            string resetUrl = $"http://localhost:5167/Login/ResetPassword?email={email}";
            System.Console.WriteLine(resetUrl);
            try
            {
                _emailService.SendResetEmail(existingUser.Email, resetUrl);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
       

        public async Task<bool> ResetPassword(ResetPasswordviewmodel model)
        {
            var existingUser = await _userRepository.GetUserByEmailFor(model.Email);

           if (model.NewPassword!=model.ConfirmPassword)
            {
                 throw new Exception("new password and confiermed new pasword does not match");
            }

            existingUser.Password = BCrypt.HashPassword(model.NewPassword, workFactor: 13);

            await _userRepository.UpdateUser(existingUser);

            return true;
        }

        public void SetJwtToken(HttpResponse response, string token)
        {
            response.Cookies.Append("jwtToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });
        }

        public void SetCookie(HttpResponse response, string email)
        {
            response.Cookies.Append("Email", email, new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7),
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Strict
            });
        }

    }
}
