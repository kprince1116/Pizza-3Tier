using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pizzashop.DAL.ViewModels
{
    public class ResetPasswordviewmodel
    {
        public string Email { get; set; }

        [Required(ErrorMessage = "NewPassword field is required")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "ConfirmedPassword field is required")]
        [Compare("NewPassword", ErrorMessage = "NewPassword and ConfirmedPassword does not match")]
        public string ConfirmPassword { get; set; }
    }
}