namespace DAL.ViewModels;

public class AddTaxviewmodel
{
    public int? TaxId { get; set; }
    public string TaxName { get; set; }
    public bool TaxType { get; set;}
    public bool IsActive { get; set;}
    public bool IsDefault { get; set;}

    public Decimal TaxValue { get; set;}
}
