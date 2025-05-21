using AspNetCoreHero.ToastNotification.Abstractions;
using BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Pizzashop.BAL.Interfaces;
using Pizzashop.DAL.ViewModels;

namespace Pizzashop.Presentation.Controllers;

public class LoginController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService;
    private readonly INotyfService _notyf;

    public LoginController(IConfiguration configuration, IAuthService authService, ITokenService tokenService, INotyfService notyf)
    {
        _configuration = configuration;
        _authService = authService;
        _tokenService = tokenService;
        _notyf = notyf;
    }

    public IActionResult Login()
    {
        if (!string.IsNullOrEmpty(Request.Cookies["Email"]))
        {
            return RedirectToAction("Index", "Home");
        }
        else if (!string.IsNullOrEmpty(Request.Cookies["jwtToken"]))
        {
            return RedirectToAction("Index", "Home");
        }
        else
        {
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Login(Loginviewmodel user)
    {

        try
        {
            var result = await _authService.AuthenticateUserAsync(user);

            _authService.SetJwtToken(Response, result);

            var userrole = _tokenService.GetRoleFromToken(result);

            if (user.RememberMe)
            {
                _authService.SetCookie(Response, user.Email);
            }

            if (userrole == "Chef")
            {
                TempData["LoginSuccess"] = true;
                return RedirectToAction("Kot", "Kot");
            }
            else
            {
                TempData["LoginSuccess"] = true;
                return RedirectToAction("Index", "Home");
            }

        }
        catch (Exception ex)
        {
            // ViewBag.ErrorMessage = ex.Message;
            _notyf.Error(ex.Message, 3);
            return View(user);
        }
    }

    public IActionResult ForgotPassword(string email, string password)
    {
        ViewData["emailview"] = email;
        return View();
    }

    [HttpPost]

    public async Task<IActionResult> ForgotPassword(string email)
    {
        var user = await _authService.SendMail(email);

        if (user == false)
        {
            ModelState.AddModelError("email", "User does not exists");
            return View();
        }
        // ViewBag.Message = "An email has been sent to reset your password.";
        _notyf.Success("An email has been sent to reset your password.", 3);
        TempData["EmailSuccess"] = true;
        return View();
    }

    public IActionResult ResetPassword(string? email, string expires)
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordviewmodel model)
    {
        try
        {
            var result = await _authService.ResetPassword(model);
            _notyf.Success("Password reset successfully", 3);
            return RedirectToAction("Login", "Login");
        }
        catch (Exception ex)
        {
             _notyf.Error(ex.Message, 3);
            return View();
        }

    }




}


