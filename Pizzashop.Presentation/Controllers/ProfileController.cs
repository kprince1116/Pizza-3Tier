using BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Pizzashop.BAL.Interfaces;
using Pizzashop.DAL.ViewModels;

namespace Pizzashop.Presentation.Controllers;

public class ProfileController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IUserDetails _userDetails;
    private readonly ITokenService _tokenService;

    public ProfileController(IConfiguration configuration, IAuthService authService, ITokenService tokenService, IUserDetails userDetails)
    {
        _configuration = configuration;
        _userDetails = userDetails;
        _tokenService = tokenService;

    }

    public async Task<IActionResult> MyProfile()
    {
        var token = Request.Cookies["jwtToken"];
        string email = _tokenService.GetEmailFromToken(token);
        var profiles = _tokenService.GetImageUrlFromToken(token);
        var profile = await _userDetails.GetUserProfile(email);
        Console.WriteLine(profile);
        return View(profile);
    }

    [HttpPost]

    public async Task<IActionResult> MyProfile(ProfileViewmodel model)
    {
        var result = await _userDetails.UpdateProfile(model);

        if (result)
        {
            TempData["ProfileUpdateSuccess"] = true;
            return RedirectToAction("Index", "Home");
        }
        else
        {
            TempData["ProfileUpdateError"] = true;
            return RedirectToAction("Index", "Home");
        }


    }
    public IActionResult GetRoles()
    {
        var roles = _userDetails.GetRoles();
        return Json(roles);
    }
    public IActionResult GetCountries()
    {
        var countries = _userDetails.GetCountries();
        return Json(countries);
    }

    public IActionResult GetStates(int countryId)
    {
        var states = _userDetails.GetStates(countryId);
        return Json(states);
    }

    public IActionResult GetCities(int stateId)
    {
        var cities = _userDetails.GetCities(stateId);
        return Json(cities);
    }

    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwtToken");
        Response.Cookies.Delete("Email");
        TempData["LogOutSuccess"] = true;
        return RedirectToAction("Login", "Login");
    }

    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordviewmodel model)
    {
        try
        {
            var token = Request.Cookies["jwtToken"];

            string email = _tokenService.GetEmailFromToken(token);

            var result = await _userDetails.ChangePassword(email, model);

            if (result == null)
            {
                TempData["ChangePasswordError"] = "Password does not changed";
                return View();
            }
            TempData["ChangePasswordSuccess"] = "Password Changed Succesfully";
            return RedirectToAction("UserList", "User");
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = ex.Message;
            return View(model);
        }
    }
}
