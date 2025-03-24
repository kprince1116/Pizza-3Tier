using DAL.Repository;
using Pizzashop.DAL.ViewModels;

namespace BAL.Models.Interfaces;

public interface ITaxesAndFessService
{
    public Task<Taxviewmodel> GetTaxDeails(int pageNo ,int  pageSize , string search );

    public Task AddTax(Taxviewmodel model);

    public Task<bool> DeleteTax(int id);

    public Task<EditTaxviewmodel> GetEditTax(int id);

    public Task<bool> EditTax(EditTaxviewmodel model);

    public Task<bool> EditTaxAvailabity(int id, bool isAvailable);
    public Task<bool> EditTaxDefault(int id, bool isAvailable);


}
