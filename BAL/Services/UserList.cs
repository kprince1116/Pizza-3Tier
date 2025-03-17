using BAL.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Hosting;

using Microsoft.Extensions.Configuration;
// using Pizzashop.DAL.Models;
using Pizzashop.DAL.ViewModels;

namespace BAL.Services;

public class UserList : IUserList
{

    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;


    public UserList(IUserRepository userRepository, IConfiguration configuration, IEmailService emailService)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _emailService = emailService;
    }

    public async Task<List<UserListviewmodel>> GetUserList(int pageNumber, int pageSize, string searchTerm, string sortDirection, string sortBy)
    {
        return await _userRepository.GetUserList(pageNumber, pageSize, searchTerm, sortDirection, sortBy);
    }

    public async Task<int> GetUsercount(string searchTerm)
    {
        return await _userRepository.GetUsercount(searchTerm);
    }

    public async Task AddUserAsync(AddUserviewmodel user)
    {

        

        string body = $@"
                       <html>
                       <body>
                          <div style='width: 40vw>
                          <h1>Welcome to PizzaShop</h1>
                          <p>Please find details below for login into your account</p>
                          <div style='border:1px solid black;'>
                          <h1>Login Details</h1>
                          <h3>Username : {user.Username}</h3>
                            <h3>Password : {user.Password}</h3>
                          </div>
                          <p>If You Encounter any issue , please don't hesitate to contact our<br> support team.</p>
                          </div>
                       </body>
                       </html> 
                        ";

        

        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        await _userRepository.AddUserAsync(user);

        await _emailService.SendEmailAsync(user.Email, "Welcome to PizzaShop", body);

    }

    public async Task<User> GetUserById(int UserId)
    {
        return await _userRepository.GetUserById(UserId);
    }

    public async Task EditUserAsync(EditUserviewmodel user)
    {
        var existingUser = await _userRepository.GetUserById(user.UserId);

        var countryname = await _userRepository.GetCountryById(user.CountryId);
        var statename = await _userRepository.GetStateById(user.StateId);
        var cityname = await _userRepository.GetCityById(user.CityId);

        existingUser.Firstname = user.Firstname;
        existingUser.Lastname = user.Lastname;
        existingUser.Username = user.Username;
        existingUser.Email = user.Email;
        existingUser.Userrole = user.Userrole;
        existingUser.Status = user.Status;
        existingUser.Country = user.CountryId;
        existingUser.State = user.StateId;
        existingUser.City = user.CityId;
        existingUser.Phonenumber = user.Phonenumber;
        existingUser.Address = user.Address;
        existingUser.Zipcode = user.Zipcode;

         if (user.ProfileImage != null)
        {
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string filename = $"{Guid.NewGuid()}_{user.ProfileImage.FileName}";
            string filepath = Path.Combine(uploadsFolder, filename);

            using (FileStream fileStream = new FileStream(filepath, FileMode.Create))
            {
                await user.ProfileImage.CopyToAsync(fileStream);
            }

            existingUser.ProfileImage = $"/uploads/{filename}"; 
            
        }

        await _userRepository.UpdateUserAsync(existingUser);
      
    }

    public Task<IEnumerable<Userrole>> GetRolesAsync()
    {
        return _userRepository.GetRolesAsync();
    }

    public Task<IEnumerable<Country>> GetCountriesAsync()
    {
        return _userRepository.GetCountryAsync();
    }
    public Task<IEnumerable<State>> GetStatesAsync()
    {
        return _userRepository.GetStateAsync();
    }

    public Task<IEnumerable<City>> GetCitiesAsync()
    {
        return _userRepository.GetCityAsync();
    }


    public async Task<bool> GetById(int UserId)
    {
        var existingUser = await _userRepository.GetUserByIdForDelete(UserId);

        if (existingUser == null)
        {
            return false;
        }
        existingUser.Isdeleted = true;
        await _userRepository.DeleteUserAsync(existingUser);
        return true;
    }

}
