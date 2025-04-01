
using DAL.Models;
using Pizzashop.DAL.ViewModels;
using Pizzashop.Data.Repositories;

namespace DAL.Interfaces;


public interface IUserRepository
{
    public Task<User> GetUserByEmailAsync(string email);

    public Task<User> GetUserByEmailWithRole(string email);

    public Task<User> GetUserByEmailFor(string email);
    Task UpdateUser(User user);

    public Task<bool> IsEmailUniqueAsync(string email);

    public Task<bool> IsPhoneNumberUniqueAsync(string phoneNumber);

    public Task<User> GetUserByEmailForProfile(String email);

    Task updateUser(User user);
    List<Country> GetCountries();
    List<State> GetStates(int countryId);
    List<City> GetCities(int stateId);

    public Task<User> GetUserByEmail(String email);

    public Task<User> GetUserByEmailForChangePassword(string email);    

    Task UpdateUserforChangePassword(User existingUser);

    public Task<List<UserListviewmodel>> GetUserList(int pageNumber,int pageSize, string searchTerm, string sortDirection, string sortBy);

    public Task<int> GetUsercount(string searchTerm);

    Task<bool> AddUserAsync(AddUserviewmodel user);

    public  Task<bool> EmailExists(string Email);

    public Task<bool> PhoneNumberExists(string Phonenumber);

    public Task<User> GetUserById(int UserId);

    Task UpdateUserAsync(User existingUser);

    public Task<IEnumerable<Userrole>> GetRolesAsync();

    public Task<IEnumerable<Country>> GetCountryAsync();

    public Task<IEnumerable<State>> GetStateAsync();

    public Task<IEnumerable<City>> GetCityAsync();

    public Task<User> GetUserByIdForDelete(int UserId);

    Task DeleteUserAsync(User existingUser);

    Task<string> GetCountryById(int id);

    Task<string> GetStateById(int id);

    Task<string> GetCityById(int id);
    
}

