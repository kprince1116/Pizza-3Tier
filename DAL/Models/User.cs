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

    public int? Userrole { get; set; }

    public virtual City? CityNavigation { get; set; }

    public virtual Country? CountryNavigation { get; set; }

    public virtual ICollection<Customer> CustomerCreatedByNavigations { get; set; } = new List<Customer>();

    public virtual ICollection<Customer> CustomerModifiedByNavigations { get; set; } = new List<Customer>();

    public virtual ICollection<Order> OrderCreatedByNavigations { get; set; } = new List<Order>();

    public virtual ICollection<Order> OrderModifiedByNavigations { get; set; } = new List<Order>();

    public virtual State? StateNavigation { get; set; }

    public virtual Userrole1? UserroleNavigation { get; set; }

    public virtual ICollection<WaitingToken> WaitingTokenCreatedByNavigations { get; set; } = new List<WaitingToken>();

    public virtual ICollection<WaitingToken> WaitingTokenModifiedByNavigations { get; set; } = new List<WaitingToken>();
}
