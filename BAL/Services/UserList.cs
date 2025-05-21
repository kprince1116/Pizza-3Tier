using BAL.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
// using Pizzashop.DAL.Models;
using Pizzashop.DAL.ViewModels;

namespace BAL.Services;

public class UserList : IUserList
{

    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    private readonly PizzaShopContext _db;



    public UserList(IUserRepository userRepository, IConfiguration configuration, IEmailService emailService ,PizzaShopContext db )
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _emailService = emailService;
        _db = db;
    }

    public async Task<List<UserListviewmodel>> GetUserList(int pageNumber, int pageSize, string searchTerm, string sortDirection, string sortBy)
    {
        return await _userRepository.GetUserList(pageNumber, pageSize, searchTerm, sortDirection, sortBy);
    }

    public async Task<int> GetUsercount(string searchTerm)
    {
        return await _userRepository.GetUsercount(searchTerm);
    }

    public async Task<bool> EmailExists(string Email)
    {
        return await _userRepository.EmailExists(Email);
    }

     public async Task<bool> PhoneNumberExists(string Phonenumber)
     {
        return await _userRepository.PhoneNumberExists(Phonenumber);
     }

    public async Task<AddUserResult> AddUserAsync(AddUserviewmodel user)
    {

        var emailexist = await _db.Users.AnyAsync(u=>u.Email.ToLower()==user.Email);
            if(emailexist)
            {
                return AddUserResult.EmailExists;
            }
            var phoneexist = await _db.Users.AnyAsync(u=>u.Phonenumber.ToLower()==user.Phonenumber);
            if(phoneexist)
            {
                return AddUserResult.PhoneExists;
            }

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

        return AddUserResult.Success;

    }

    public async Task<EditUserviewmodel> GetUserById(int UserId)
    {
        var user = await _userRepository.GetUserById(UserId);

        var countryname = await _userRepository.GetCountryById(user.Country.Value);
        var statename = await _userRepository.GetStateById(user.State.Value);
        var cityname = await _userRepository.GetCityById(user.City.Value);
        var role = await _userRepository.GeRoleById(user.Userrole.Value);

         var viewModel = new EditUserviewmodel
        {
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Username = user.Username,
            Email = user.Email,
            Country = countryname,
            image = user.ProfileImage,
            Status = user.Status,
            State = statename,
            City = cityname,
            Userrole = user.Userrole.Value,
            UserRoleName =role,
            Zipcode = user.Zipcode,
            Address = user.Address,
            Phonenumber = user.Phonenumber,
            CityId=user.City.Value,
            StateId=user.State.Value,
            CountryId=user.Country.Value,
        };
        return viewModel;
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
        // existingUser.ProfileImage = user.image;

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

    public Task<IEnumerable<Userrole1>> GetRolesAsync()
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
