using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class State
{
    public int Stateid { get; set; }

    public string Statename { get; set; } = null!;

    public int? Country { get; set; }

    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    public virtual Country? CountryNavigation { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
