using DAL.Models;
using Pizzashop.DAL.ViewModels;

namespace BAL.Interfaces;

public interface IUserDetails
{
    public Task<ProfileViewmodel> GetUserProfile(string email);
    public Task<bool> UpdateProfile(ProfileViewmodel model);
    public List<Country> GetCountries();
    public List<State> GetStates(int countryId);
    public List<City> GetCities(int stateId);
    public Task<ChangePasswordviewmodel> ChangePassword(string email , ChangePasswordviewmodel model);
}
