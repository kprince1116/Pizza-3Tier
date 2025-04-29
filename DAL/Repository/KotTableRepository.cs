using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Pizzashop.DAL.ViewModels;

namespace DAL.Repository;

public class KotTableRepository : IKotTableRepository
{
    private readonly PizzaShopContext _db;

    public KotTableRepository(PizzaShopContext db)
    {
        _db = db;
    }

    public async Task<List<Section>> GetSections()
    {
        return await _db.Sections.Where(u => u.Isdeleted == false).OrderBy(u => u.Sectionid).ToListAsync();
    }

    public async Task<List<Table>> GetTablesBySectionIdAsync(int sectionId)
    {
        return await _db.Tables.Where(t => t.Sectionid == sectionId && t.Isdeleted == false)
                                        .Include(u=>u.Customer).ThenInclude(u=>u.OrderTables)
                                        .Include(u=>u.OrderTables.Where(u=>u.IsDeleted == false))
                                        .ThenInclude(u=>u.Order)
                                        .OrderBy(u=>u.Tableid).ToListAsync();
    }

    public async Task<List<Models.Section>> GetSectionList()
    {
        return await _db.Sections.Where(u => u.Isdeleted == false).OrderBy(u=>u.Sectionid).ToListAsync();
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

        public async Task<List<waitingtokenviewmodel>> GetWaitingList(int id)
        {
            var waitinglist = await _db.WaitingTokens.Include(u=>u.Customer).Where(u=> u.SectionId == id && u.IsDeleted == false && u.IsAssigned == false )
            .Select(u=> new waitingtokenviewmodel
            {
               Id = u.Id,
               Name = u.Customer.Customername,
               NoOfPerson =(int) u.Customer.TotalPersons
            }).ToListAsync();

            return waitinglist;
        }

    public async Task<WaitingToken> GetCustomerDetails(int id)
    {
        var customer = await _db.WaitingTokens
        .Where(u => u.Id == id && u.IsDeleted == false)
        .Include(u=>u.Customer) 
        .Include(u=>u.Section)
        .FirstOrDefaultAsync();
        
        return customer;
    }
    public async  Task<WaitingToken> GetCustomerDetailsForAssign(int customerId)
    {
        var customer = await _db.WaitingTokens
        .Where(u => u.Customerid == customerId && u.IsDeleted == false)
        .Include(u=>u.Customer) 
        .Include(u=>u.Section)
        .FirstOrDefaultAsync();
        
        return customer;
    }

    public async Task<Customer> GetCustomerDetailsByEmail(string email)
    {
        var customer = await _db.Customers
            .Include(u => u.WaitingTokens)
            .FirstOrDefaultAsync(u => u.Customeremail == email && u.Isdelete == false);

        return customer;
    }

    public async Task UpdateTables(Table tables)
    {
        try
        {
         _db.Tables.Update(tables);
        await _db.SaveChangesAsync();
        }
        catch (Exception e)
        {
            
            Console.WriteLine(e);
        }
       
    }
    public async Task AddCustomer(Customer newcustomer)
    {
        try
        {
             _db.Customers.Add(newcustomer);
            await _db.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

     public async Task UpdateCustomer(WaitingToken model)
     {
        _db.WaitingTokens.Update(model);
        await _db.SaveChangesAsync();
     }

    public async Task<Table> GetTablesByIdAsync(int tableId)
    {
        return await _db.Tables.FirstOrDefaultAsync(u => u.Tableid == tableId && u.Isdeleted == false);
    }

    public async Task<Customer> GetCustomerFromCustomerTable(int id)
    {
        return await _db.Customers.FirstOrDefaultAsync(u => u.Customerid == id && u.Isdelete == false);
    }

     public async Task<Order> GenerateOrder(Order obj){
            await _db.Orders.AddAsync(obj);
            await _db.SaveChangesAsync();
            return obj;
        }

    public async Task<OrderTable> AddOrderTable(OrderTable orderTable)
    {
        await _db.OrderTables.AddAsync(orderTable);
        await _db.SaveChangesAsync();
        return orderTable;
    }
   
}
