namespace Pizzashop.DAL.ViewModels;

public class ProfileViewmodel
{
    public string Firstname { get; set; } = null!;
    public string Lastname { get; set; } = null!;
    public string? Username { get; set; }
    public string? Email {get; set;} = null!;
    public string Phonenumber { get; set; } = null!;

    public string ProfileImage {get; set;}

    public string? CountryName { get; set; }

    public string? StateName { get; set; }

    public string? CityName { get; set; }
    public int? Country { get; set; }
    public int? State { get; set; }
    public int? City { get; set; }
    public string Address { get; set; } = null!;
    public int Zipcode { get; set; }
    public string? userrole {get ; set;} 
}
