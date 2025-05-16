using System.Threading.Tasks;
using Pizzashop.DAL.ViewModels;

namespace BAL.Interfaces;

public interface ITokenService
{
    public string GetEmailFromToken(string token);
    public  Task<int> GetIdFromToken(string token);
    public string GetImageUrlFromToken(string token);
    public string GetRoleFromToken(string token);
    
}
