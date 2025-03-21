using DAL.Models;
using Pizzashop.DAL.ViewModels;

namespace DAL.Interfaces;

public interface ITaxesAndFessRepository
{
   public Task<Taxviewmodel> GetAllTaxDetailsAsync(int pageNo,int  pageSize, string search);

   Task AddTax (Taxviewmodel model);

   Task<Taxesandfess> DeleteTax(int id);

   Task UpdateTaxAsync(Taxesandfess existingTax);

   Task<EditTaxviewmodel> GetEditTax(int id);

    // Task<Taxesandfess> EditTaxId(int TaxId);

    Task<Taxesandfess> GetTaxById(int taxId);

}
