using BAL.Models.Interfaces;
using DAL.Interfaces;
using Pizzashop.DAL.ViewModels;

namespace BAL.Services;

public class TaxesAndFessService : ITaxesAndFessService
{
    private readonly ITaxesAndFessRepository  _TaxesAndFessRepository;

    public TaxesAndFessService ( ITaxesAndFessRepository  TaxesAndFessRepository)
    {
        _TaxesAndFessRepository = TaxesAndFessRepository ;

    }

    public async Task<Taxviewmodel> GetTaxDeails(int pageNo ,int  pageSize , string search )
    {
        var taxData = await _TaxesAndFessRepository.GetAllTaxDetailsAsync(pageNo, pageSize, search);
        return taxData;
    }

     public async Task AddTax(Taxviewmodel model)
     {
        await _TaxesAndFessRepository.AddTax(model);
     }

    public async Task<bool> DeleteTax(int id)
    {
        var existingTax = await _TaxesAndFessRepository.DeleteTax(id);

        if(existingTax == null)
        {
            return false;
        }

        existingTax.Isdeleted = true;

        await _TaxesAndFessRepository.UpdateTaxAsync(existingTax);
        return true;
    }

    public async Task<EditTaxviewmodel> GetEditTax(int id)
    {
        return await _TaxesAndFessRepository.GetEditTax(id);
    }

    public async Task<bool> EditTax(EditTaxviewmodel model)
    {
        var existingTax = await _TaxesAndFessRepository.GetTaxById(model.TaxId);
    
        if(existingTax == null)
        {
            return false;
        }
    
        existingTax.Taxid = model.TaxId;
        existingTax.Taxvalue = model.TaxValue;
        existingTax.Taxname = model.TaxName;
        existingTax.TaxType = model.TaxType;
        existingTax.Isactive =  model.IsActive;
        existingTax.Isdefault =  model.IsDefault;
    
        await _TaxesAndFessRepository.UpdateTaxAsync(existingTax);
    
        return true;
    }

}
