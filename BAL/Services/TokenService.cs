using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BAL.Interfaces;
using Microsoft.VisualBasic;

namespace BAL.Services;

public class TokenService : ITokenService
{
    public string GetEmailFromToken(string token)
    {

        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);

        if(jsonToken !=null)
        {
            var email = jsonToken.Claims.FirstOrDefault(c=> c.Type == ClaimTypes.Email).Value;
            System.Console.WriteLine("email:"+email);
            if(email != null)
            {
                return email;
            }
        }

        return null;
    }

}
