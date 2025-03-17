using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pizzashop.DAL.ViewModels
{
public class Loginviewmodel
{   
    // [Required(ErrorMessage = "Email Field is Required")]
    public string Email { get; set; } = null!;

    // [Required(ErrorMessage = "Password Field is Required")]
    public string Password { get; set; } = null!;
    public bool RememberMe {get ; set;} 
    public string Role { get; set; } = null!;

}
}