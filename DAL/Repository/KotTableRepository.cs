using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Pizzashop.DAL.ViewModels;
using static System.Collections.Specialized.BitVector32;

namespace DAL.Repository;

public class KotTableRepository : IKotTableRepository
{
    private readonly PizzaShopContext _db;

    public KotTableRepository(PizzaShopContext db)
    {
        _db = db;
    }

    public async Task<List<Models.Section>> GetSections()
    {
        return await _db.Sections.Where(u => u.Isdeleted == false).OrderBy(u => u.Sectionid).ToListAsync();
    }

    public async Task<List<Table>> GetTablesBySectionIdAsync(int sectionId)
    {
        return await _db.Tables .Where(t => t.Sectionid == sectionId && t.Isdeleted == false).OrderBy(u=>u.Tableid).ToListAsync();
    }

    public async Task<List<Models.Section>> GetSectionList()
    {
        return await _db.Sections.Where(u => u.Isdeleted == false).ToListAsync();
    }
    public async Task AddWaitingToken(waitingtokenviewmodel model)
    {
        try
        {
            var existingcustomer = await _db.Customers.FirstOrDefaultAsync(u => u.Customeremail == model.Email);

            if(existingcustomer!=null)
            {
                existingcustomer.Customername = model.Name;
                existingcustomer.Phonenumber = model.Phone;
                existingcustomer.TotalPersons = model.NoOfPerson;
                _db.Customers.Update(existingcustomer);

                var waitingToken = new WaitingToken
            {
                Customerid = existingcustomer?.Customerid,
                SectionId = model.sectionId,
                NoOfPersons = model.NoOfPerson,
            };

            _db.WaitingTokens.Add(waitingToken);

            }
        
            else
            {
                var customer = new Customer
                {
                    Customername = model.Name,
                    Customeremail = model.Email,
                    Phonenumber = model.Phone,
                    TotalPersons = model.NoOfPerson,
                    CreatedDate = DateTime.Now,
                };
                await _db.Customers.AddAsync(customer);
                _db.SaveChanges();

                var waitingToken = new WaitingToken
                {
                    Customerid = customer.Customerid,
                    SectionId = model.sectionId,
                    NoOfPersons = model.NoOfPerson,
                };
                _db.WaitingTokens.Add(waitingToken);
            }

            _db.SaveChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

        public async Task<List<waitingtokenviewmodel>> GetWaitingList()
        {
            var waitinglist = await _db.WaitingTokens.Include(u=>u.Customer).Where(u=>u.IsDeleted == false)
            .Select(u=> new waitingtokenviewmodel
            {
               Id = u.Id,
               Name = u.Customer.Customername,
               NoOfPerson =(int) u.Customer.TotalPersons
            }).ToListAsync();

            return waitinglist;
        }
   
}
