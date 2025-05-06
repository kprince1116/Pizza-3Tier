using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace BAL.Services;

public class TokenService : ITokenService

{
    private readonly PizzaShopContext _db;

    public TokenService(PizzaShopContext db)
    {
        _db = db;
    }


    public string GetEmailFromToken(string token)
    {

        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);

        if (jsonToken != null)
        {
            var email = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;

            if (email != null)
            {
                return email;
            }
        }

        return null;
    }

    public  string GetImageUrlFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);

        var imageUrlClaim =  jsonToken.Claims.FirstOrDefault(c => c.Type == "imageUrl");

        return imageUrlClaim.Value;
           
    }
    public  string GetRoleFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);

        var Role =  jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;

        return Role;
           
    }


    public async Task<int> GetIdFromToken(string token)
    {

        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);

        if (jsonToken != null)
        {
            var email = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user != null)
            {
                return user.UserId;
            }
        }

        return 0;
    }


}
