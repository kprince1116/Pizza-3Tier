using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

// using Pizzashop.DAL.Models;

using Pizzashop.DAL.ViewModels;

namespace Pizzashop.BAL.Interfaces
{
    public interface IAuthService
    {
        public Task<string> AuthenticateUserAsync(Loginviewmodel model);
        public Task<bool> SendMail(String email);
        public Task<bool> ResetPassword(ResetPasswordviewmodel model);
        void SetJwtToken(HttpResponse response, string token);
        void SetCookie(HttpResponse response, string email);
    }
}

