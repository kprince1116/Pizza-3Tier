using System.ComponentModel.DataAnnotations;
using DAL.Models;

namespace Pizzashop.DAL.ViewModels;

public class waitingtokenviewmodel
{
    public int Id { get; set; }
    public int customerId { get; set; }

    public int OrderId { get; set; }

    [Required(ErrorMessage = "Please enter your email address.")]
    public string Email {get; set;}

    [Required(ErrorMessage = "Please enter your name")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Please enter your phone number")]
    [StringLength(10, ErrorMessage = "Phone number must be 10 digits.")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "Please enter the total person")]
    public int NoOfPerson { get; set; }

    public DateTime CreatedAt { get; set; }

    public int tableId { get; set; }

    public int sectionId { get; set; }

    public string sectionName { get; set; }

    public List<Section> sections { get; set; }

}
