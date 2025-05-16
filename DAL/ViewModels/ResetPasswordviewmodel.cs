using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pizzashop.DAL.ViewModels
{
    public class ResetPasswordviewmodel
    {
        public string Email { get; set; }

        [Required(ErrorMessage = "NewPassword field is required")]
        [StringLength(100, ErrorMessage = "NewPassword must be at least 6 characters long.", MinimumLength = 6)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "ConfirmedPassword field is required")]
        [StringLength(100, ErrorMessage = "ConfirmedPassword must be at least 6 characters long.", MinimumLength = 6)]
        [Compare("NewPassword", ErrorMessage = "NewPassword and ConfirmedPassword does not match")]
        public string ConfirmPassword { get; set; }
    }
}