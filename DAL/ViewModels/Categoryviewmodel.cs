using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using DAL.Models;

namespace  Pizzashop.DAL.ViewModels;
public class Categoryviewmodel

{
     public int CategoryId { get; set; }

     [Required(ErrorMessage = "Name is required")]
     public string CategoryName { get; set; } = null!;

     [Required(ErrorMessage = "Description is required")]
    public string Description { get; set; } = null!;

}