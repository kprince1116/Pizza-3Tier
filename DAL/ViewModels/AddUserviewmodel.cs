using System.ComponentModel.DataAnnotations;
using DAL.Models;
using Microsoft.AspNetCore.Http;

namespace Pizzashop.DAL.ViewModels;

public class AddUserviewmodel
{
    public int UserId { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    public string Firstname { get; set; } = null!;

    [Required(ErrorMessage = "Last name is required.")]
    public string Lastname { get; set; } = null!;

    [Required(ErrorMessage = "Email is required.")]
    public string Email { get; set; } = null!;

     [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "User role is required.")]
    public int Userrole { get; set; }

    [Required(ErrorMessage = "Phone number is required.")]
    public string Phonenumber { get; set; } = null!;

    [Required(ErrorMessage = "Profile image is required.")]
    public IFormFile ProfileImage { get; set; } 

    public int? Country { get; set; }

    public int? State { get; set; }

    public int? City { get; set; }

    [Required(ErrorMessage = "Zip code is required.")]
    public int Zipcode { get; set; }

    [Required(ErrorMessage = "Address is required.")]
    public string Address { get; set; } = null!;

    public bool Isdeleted { get; set; }

    [Required(ErrorMessage = "UserName is required.")]
    public string? Username { get; set; }

    public bool? Status { get; set; }

    public virtual City? CityNavigation { get; set; }

    public virtual Country? CountryNavigation { get; set; }

    public virtual State? StateNavigation { get; set; }

    public virtual Userrole UserroleNavigation { get; set; } = null!;
}
