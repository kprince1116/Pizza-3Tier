using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pizzashop.DAL.ViewModels;
public class UserListviewmodel
{
    
    public int UserId { get; set; }
    public string Firstname { get; set; } = null!;
    public string Lastname { get; set; } = null!;
    public string? Email { get; set; } = null!;
    public string ProfileImage {get ; set;}
    public string Phonenumber { get; set; } = null!;
    public bool Status { get; set; }
    public string? userrole { get; set; } = null!;

}
