
// using DAL.Models;
using DAL.Models;
// using Pizzashop.DAL.Models;
using Pizzashop.DAL.ViewModels;

namespace BAL.Interfaces;

public interface IUserList
{
    public Task<List<UserListviewmodel>> GetUserList(int pageNumber, int pageSize, string searchTerm, string sortDirection, string sortBy);
    public Task<int> GetUsercount(string searchTerm);
    public Task<AddUserResult> AddUserAsync(AddUserviewmodel user);
    public Task<bool> EmailExists(string Email);

    public Task<bool> PhoneNumberExists(string Phonenumber);

    public Task<EditUserviewmodel> GetUserById(int UserId);
    // Task<string?> GetUserById(int? userId);

    public Task<IEnumerable<Country>> GetCountriesAsync();
    public Task<IEnumerable<State>> GetStatesAsync();

    public Task<IEnumerable<City>> GetCitiesAsync();

     Task<IEnumerable<Userrole1>> GetRolesAsync(); 

    public Task EditUserAsync(EditUserviewmodel user);

    public Task<bool> GetById(int UserId);
}
