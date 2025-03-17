using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace  Pizzashop.DAL.ViewModels;
public class ChangePasswordviewmodel
{
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "CurrentPassword field is required")]
    public string CurrentPassword { get; set; } = null!;

    [Required(ErrorMessage = "NewPassword field is required")]
    public string NewPassword { get; set; } = null!;

    [Required(ErrorMessage = "ConfirmedNewPassword field is required")]
    [Compare("NewPassword", ErrorMessage = "Password does not match")]
    public string ConfirmNewPassword { get; set; } = null!;

}