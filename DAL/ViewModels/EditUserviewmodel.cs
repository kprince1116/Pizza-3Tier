using System.ComponentModel.DataAnnotations;
using DAL.Models;
using Microsoft.AspNetCore.Http;

namespace Pizzashop.DAL.ViewModels;

public class EditUserviewmodel
{
    public int UserId { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    public string Firstname { get; set; } = null!;

    [Required(ErrorMessage = "Last name is required.")]
    public string Lastname { get; set; } = null!;

    [Required(ErrorMessage = "Email is required.")]
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int Userrole { get; set; }

    public string? UserRoleName {get; set;}

    [Required(ErrorMessage = "Phone number is required.")]
    [StringLength(10, ErrorMessage = "Phone number must be 10 digits.")]
    public string Phonenumber { get; set; } = null!;

    public IFormFile ProfileImage { get; set; } 

    public string image { get; set; }

     public int CountryId { get; set; }

    public int StateId { get; set; }

    public int CityId { get; set; }

    public string? Country { get; set; }

    public string? State { get; set; }

    public string? City { get; set; }

    [Required(ErrorMessage = "Zip code is required.")]
    public int Zipcode { get; set; }

    [Required(ErrorMessage = "Address is required.")]
    public string Address { get; set; } = null!;

    public bool Isdeleted { get; set; }

    public string? Username { get; set; }

    public bool? Status { get; set; }

    public IEnumerable<Userrole1> roles { get; set; } 
    public IEnumerable<Country> countrylist { get; set; } 
    public IEnumerable<State> statelist { get; set; } 
    public IEnumerable<City> citylist { get; set; } 

    // public virtual City? CityNavigation { get; set; }

    // public virtual Country? CountryNavigation { get; set; }

    // public virtual State? StateNavigation { get; set; }

    // public virtual Userrole UserroleNavigation { get; set; } = null!;
}
