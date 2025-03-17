using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int Userrole { get; set; }

    public string Phonenumber { get; set; } = null!;

    public string? ProfileImage { get; set; }

    public string? CreatedBy { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? Country { get; set; }

    public int? State { get; set; }

    public int? City { get; set; }

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
