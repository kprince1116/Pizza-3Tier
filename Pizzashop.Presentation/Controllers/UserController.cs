using BAL.Attributes;
using BAL.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;


// using DAL.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Pizzashop.DAL.ViewModels;
// using Pizzashop.DAL.Models;

namespace Pizzashop.Presentation.Controllers;

public class UserController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IUserList _userList;
    private readonly IUserRepository _userRepository;
    private readonly IUserDetails _userDetails;
    private readonly IHubContext<NotificationHub> _hubcontext;

    public UserController(IConfiguration configuration, IUserList userList , IUserRepository userRepository , IUserDetails userDetails , IHubContext<NotificationHub> hubcontext)
    {
        _hubcontext = hubcontext;
        _configuration = configuration;
        _userList = userList;
        _userRepository = userRepository;
        _userDetails = userDetails;
    }

    [Authorize(Roles = "Account_Manager,Admin")]
    [_AuthPermissionAttribute("Users", ActionPermissions.CanView)]
    public async Task<IActionResult> UserList(int pageNumber = 1, int pageSize = 7, string searchTerm = "", string sortDirection = "asc", string sortBy = "name") 
    {

        var userList = await _userList.GetUserList(pageNumber, pageSize, searchTerm, sortDirection, sortBy);
        int userCount = await _userList.GetUsercount(searchTerm);

        Console.WriteLine(userCount);
        Console.WriteLine(userList.Count);

        ViewBag.TotalPages = (int)Math.Ceiling((double)userCount / pageSize);
        ViewBag.CurrentPage = pageNumber;
        ViewBag.PageSize = pageSize;
        ViewBag.TotalItems = userCount;
        ViewBag.SearchTerm = searchTerm;
        ViewBag.SortDirection = sortDirection;
        ViewBag.SortBy = sortBy;

        return View(userList);

    }

    [_AuthPermissionAttribute("Users", ActionPermissions.CanAddEdit)]
    public IActionResult AddUser()
    {
        return View();
    }

    [_AuthPermissionAttribute("Users", ActionPermissions.CanAddEdit)]
    [HttpPost]
    public async Task<IActionResult> AddUser(AddUserviewmodel user)
    {

        string token = Request.Cookies["jwtToken"];

        var users = await _userList.AddUserAsync(user);
         if(users == AddUserResult.Success)
         {
         await _hubcontext.Clients.All.SendAsync("UserMessage", "A kot was updated succesfully.");
         TempData["AddUserSuccess"] = true;
         return RedirectToAction("UserList", "User");
         }
         else if (users == AddUserResult.EmailExists)
         {
        TempData["AddUserError"] = "A user with the same email already exists.";
        return RedirectToAction("UserList", "User");
         } 
        else if (users == AddUserResult.PhoneExists)
         {
        TempData["AddUserError"] = "A user with the same Phone number already exists.";
        return RedirectToAction("UserList", "User");
         } 
          TempData["AddUserError"] = "Unknown error.";
         return RedirectToAction("UserList", "User");
    }

     [_AuthPermissionAttribute("Users", ActionPermissions.CanAddEdit)]
    public async Task<IActionResult> EditUser(int UserId)
    {
        var user = await _userList.GetUserById(UserId);
        if (user == null)
        {
            return NotFound();
        }

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

        viewModel.roles = await _userList.GetRolesAsync();
        viewModel.countrylist = await _userList.GetCountriesAsync();
        viewModel.statelist = await _userList.GetStatesAsync();
        viewModel.citylist = await _userList.GetCitiesAsync();

        // ViewData["Country"] = new SelectList(await _userList.GetCountriesAsync(), "Countryid", "Countryname", viewModel.Country);
        // ViewData["State"] = new SelectList(await _userList.GetStatesAsync(), "Stateid", "Statename", viewModel.State);
        // ViewData["City"] = new SelectList(await _userList.GetCitiesAsync(), "Cityid", "Cityname", viewModel.City);
        // ViewData["Userrole"] = new SelectList(await _userList.GetRolesAsync(), "Roleid", "Rolename", viewModel.Userrole);


        return View(viewModel);
    }

    [_AuthPermissionAttribute("Users", ActionPermissions.CanAddEdit)]
    [HttpPost]

    public async Task<IActionResult> EditUser(EditUserviewmodel user)
    {
       await _userList.EditUserAsync(user);
    
        TempData["EditUserSuccess"] = true;
        await _hubcontext.Clients.All.SendAsync("UserMessage", "A kot was updated succesfully.");
        return RedirectToAction("userList", "User");
    }

    [_AuthPermissionAttribute("Users", ActionPermissions.CanDelete)]
    [HttpPost]
    public async Task<IActionResult> SoftDelete(int UserId)
    {
        var existinguser = await _userList.GetById(UserId);
        if(existinguser)
        {
        await _hubcontext.Clients.All.SendAsync("UserMessage", "A kot was updated succesfully.");
        TempData["DeleteUserSuccess"] = true;
        return RedirectToAction("UserList", "User");
        }
        else
        {
            TempData["DeleteUserError"] = "User not found.";
            return RedirectToAction("UserList", "User");
        }
    }

}
