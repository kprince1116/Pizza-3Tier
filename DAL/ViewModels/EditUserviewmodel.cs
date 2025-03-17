using DAL.Models;
using Microsoft.AspNetCore.Http;

namespace Pizzashop.DAL.ViewModels;

public class EditUserviewmodel
{
    public int UserId { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int Userrole { get; set; }

    public string Phonenumber { get; set; } = null!;

    public IFormFile ProfileImage { get; set; } 

     public int CountryId { get; set; }

    public int StateId { get; set; }

    public int CityId { get; set; }

    public string? Country { get; set; }

    public string? State { get; set; }

    public string? City { get; set; }

    public int Zipcode { get; set; }

    public string Address { get; set; } = null!;

    public bool Isdeleted { get; set; }

    public string? Username { get; set; }

    public bool? Status { get; set; }

    public virtual City? CityNavigation { get; set; }

    public virtual Country? CountryNavigation { get; set; }

    public virtual State? StateNavigation { get; set; }

    public virtual Userrole UserroleNavigation { get; set; } = null!;
}
