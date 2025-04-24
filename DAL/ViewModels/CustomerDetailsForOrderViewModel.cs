using System.ComponentModel.DataAnnotations;

namespace Pizzashop.DAL.ViewModels;

public class CustomerDetailsForOrderViewModel
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "Customer Name is required")]
    public string CustomerName { get; set; }

    [Required(ErrorMessage = "PhoneNumber is required")]
    public string CustomerPhone { get; set; }

    [Required(ErrorMessage = "Email is required")]
    public string CustomerEmail { get; set; }

    [Required(ErrorMessage = "NoOfPerson is required")]
    public int NoOfPersons { get; set; }
}
