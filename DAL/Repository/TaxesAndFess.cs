using System.Security;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pizzashop.DAL.ViewModels;

namespace DAL.Repository;

public class TaxesAndFess : ITaxesAndFessRepository
{

      private readonly PizzaShopContext _db;

    public TaxesAndFess(PizzaShopContext db)
    {
        _db = db;
    }
    public async Task<Taxviewmodel> GetAllTaxDetailsAsync(int pageNo,int  pageSize, string search)
    {

        var taxes = _db.Taxesandfesses.Where(U=>U.Isdeleted == false &&  U.Taxname.ToLower().Contains(search.ToLower()) ).Select (
            u=> new TaxListviewmodel
            {
                TaxId = u.Taxid,
                TaxName = u.Taxname ,
                TaxType =(bool) u.TaxType,
                IsActive =(bool) u.Isactive,
                IsDefault =(bool) u.Isdefault,
                TaxValue =(decimal) u.Taxvalue ,
            }
        ).OrderBy(u=>u.TaxId);

         var totalRecords = taxes.Count();

         var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

         List<TaxListviewmodel> taxesList = taxes.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

    
         return new Taxviewmodel
        {
            TaxList = taxesList,
            CurrentPage = pageNo,
            TotalPages  = totalPages,
            PageSize = pageSize ,
            TotalRecords = totalRecords,
        };

    }

    public async Task<bool> AddTax (Taxviewmodel model)
    {
        var exists = await _db.Taxesandfesses.AnyAsync(u=>u.Taxname.ToLower()==model.AddTax.TaxName.ToLower());

        if(exists)
        {
            return false;
        }

        var tax = new Taxesandfess
        {
            Taxname =  model.AddTax.TaxName ,
            TaxType = model.AddTax.TaxType ,
            Taxvalue = model.AddTax.TaxValue ,
            Isactive = model.AddTax.IsActive,
            Isdefault = model.AddTax.IsDefault
        };
        _db.Taxesandfesses.Update(tax);
        _db.SaveChangesAsync();

        return true;

    }

    public async Task<Taxesandfess> DeleteTax(int id)
    {
        return await _db.Taxesandfesses.FirstOrDefaultAsync(u=>u.Taxid == id);
    }

    public async Task UpdateTaxAsync(Taxesandfess existingTax)
    {
        _db.Taxesandfesses.Update(existingTax);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> GetTaxName(string TaxName)
    {
        var exists = await _db.Taxesandfesses.AnyAsync(u=>u.Taxname == TaxName);
        if(exists)
        {
            return false;
        }
        return true;
    }
    public async Task<EditTaxviewmodel> GetEditTax(int id)
    {
        return await _db.Taxesandfesses.Where(u=>u.Taxid == id).Select(
            u => new EditTaxviewmodel
            {
                TaxId = u.Taxid,
                TaxName = u.Taxname ,
                TaxType =(bool) u.TaxType ,
                TaxValue =(decimal) u.Taxvalue ,
                IsActive = (bool )u.Isactive ,
                IsDefault = (bool) u.Isdefault
            }
        ).FirstOrDefaultAsync();
    }

    public async Task<Taxesandfess> GetTaxById(int TaxId)
    {
        return await _db.Taxesandfesses.FirstOrDefaultAsync(u=>u.Taxid == TaxId);
    }

}
