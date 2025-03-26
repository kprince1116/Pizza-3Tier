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

     public async Task<bool> AddTax(Taxviewmodel model)
     {
        if(model == null)
        {
            return false;
        }
         await _TaxesAndFessRepository.AddTax(model);
        return true;
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

    public async Task<bool> EditTaxAvailabity(int id, bool isAvailable)
    {
        var existingtax = await _TaxesAndFessRepository.GetTaxById(id);

        if(existingtax == null)
        {
            return false;
        }

        existingtax.Isactive = isAvailable;

        await _TaxesAndFessRepository.UpdateTaxAsync(existingtax);

        return true;

    }
    public async Task<bool> EditTaxDefault(int id, bool isAvailable)
    {
        var existingtax = await _TaxesAndFessRepository.GetTaxById(id);

        if(existingtax == null)
        {
            return false;
        }

        existingtax.Isdefault = isAvailable;

        await _TaxesAndFessRepository.UpdateTaxAsync(existingtax);

        return true;

    }



}
