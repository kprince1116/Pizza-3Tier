using BAL.Interfaces;
using DAL.Interfaces;
using DAL.Models;

// using DAL.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pizzashop.DAL.ViewModels;
// using Pizzashop.DAL.Models;

namespace Pizzashop.Presentation.Controllers;

public class UserController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IUserList _userList;

    private readonly IUserRepository _userRepository;

    public UserController(IConfiguration configuration, IUserList userList , IUserRepository userRepository)
    {
        _configuration = configuration;
        _userList = userList;
        _userRepository = userRepository;
    }

    public async Task<IActionResult> UserList(int pageNumber = 1, int pageSize = 3, string searchTerm = "", string sortDirection = "asc", string sortBy = "name") 
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

    public IActionResult AddUser()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddUser(AddUserviewmodel user)
    {
        string token = Request.Cookies["jwtToken"];

        await _userList.AddUserAsync(user);

        TempData["AddUserSuccess"] = true;

        return RedirectToAction("UserList", "User");

    }

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
            Userrole = user.Userrole,
            Zipcode = user.Zipcode,
            Address = user.Address,
            Phonenumber = user.Phonenumber,
            CityId=user.Country.Value,
            StateId=user.State.Value,
            CountryId=user.City.Value,
        };


        ViewData["Country"] = new SelectList(await _userList.GetCountriesAsync(), "Countryid", "Countryname", viewModel.Country);
        ViewData["State"] = new SelectList(await _userList.GetStatesAsync(), "Stateid", "Statename", viewModel.State);
        ViewData["City"] = new SelectList(await _userList.GetCitiesAsync(), "Cityid", "Cityname", viewModel.City);
        ViewData["Userrole"] = new SelectList(await _userList.GetRolesAsync(), "Roleid", "Rolename", viewModel.Userrole);


        return View(viewModel);
    }

    [HttpPost]

    public async Task<IActionResult> EditUser(EditUserviewmodel user)
    {
        await _userList.EditUserAsync(user);

        TempData["EditUserSuccess"] = true;

        return RedirectToAction("userList", "User");
    }

    [HttpPost]

    public async Task<IActionResult> SoftDelete(int UserId)
    {
        var existinguser = await _userList.GetById(UserId);
        TempData["DeleteUserSuccess"] = true;
        return RedirectToAction("UserList", "User");
    }

}
