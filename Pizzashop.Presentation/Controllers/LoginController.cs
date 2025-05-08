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

    public LoginController(IConfiguration configuration, IAuthService authService , ITokenService tokenService)
    {
        _configuration = configuration;
        _authService = authService;
        _tokenService = tokenService;
    }

    public IActionResult Login()
    {
        if (!string.IsNullOrEmpty(Request.Cookies["Email"]))
        {
            return RedirectToAction("Index", "Home");
        }
        
        return View();
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
            ViewBag.ErrorMessage = ex.Message;
            return View(user);
        }
    }

    public IActionResult ForgotPassword()
    {
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
        ViewBag.Message = "An email has been sent to reset your password.";
        TempData["EmailSuccess"] = true;

        return View();
    }

    public IActionResult ResetPassword(string? email)
    {
        return View(); 

    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordviewmodel model)
    {
       
        var result = await _authService.ResetPassword(model);

        return RedirectToAction("Login", "Login");
        
      
    }




}


