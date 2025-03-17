using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class City
{
    public int Cityid { get; set; }

    public string Cityname { get; set; } = null!;

    public int? State { get; set; }

    public virtual State? StateNavigation { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
