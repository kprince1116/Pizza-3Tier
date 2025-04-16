
using System.ComponentModel.DataAnnotations;
using DAL.Models;

namespace Pizzashop.DAL.ViewModels;

public class CustomerDetailsForTableviewmodel
{
    public int Id { get; set; }
    public int customerId { get; set; }
    
    [Required(ErrorMessage = "Please enter the email address.")]
    public string Email {get; set;}

    [Required(ErrorMessage = "Please enter the name")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Please enter the phone number")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "Please enter the total person")]
    public int NoOfPerson { get; set; }

    public int sectionId { get; set; }

    public string sectionName { get; set; }

    public List<Section> sections { get; set; }
}
