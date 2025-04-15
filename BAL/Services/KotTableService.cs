using BAL.Models.Interfaces;
using DAL.Interfaces;
using Pizzashop.DAL.ViewModels;
namespace BAL.Services;
using DAL.Enum;
using DAL.Models;
using static DAL.Enum.Enum;

public class KotTableService : IKotTableService
{
    private readonly IKotTableRepository _kotTableRepository;
    

    public KotTableService(IKotTableRepository kotTableRepository)
    {
        _kotTableRepository = kotTableRepository;
    }
    public async Task<List<KotTableviewmodel>> GetSections()
    {
        var sections = await _kotTableRepository.GetSections();

        var Table = new List<KotTableviewmodel>();

         foreach (var section in sections)
    {
        var tables = await _kotTableRepository.GetTablesBySectionIdAsync(section.Sectionid);
     
        var kotTableViewModel = new KotTableviewmodel
        {
            SectionId = section.Sectionid,
            SectionName = section.SectionName,
            AvailableCount = tables.Count(t=>t.Status == "Available"),
            RunningCount = tables.Count(t=>t.Status == "Running"),
            AssignedCount = tables.Count(t=>t.Status == "Assigned"),
            Tables = tables.Select(t => new OrderAppTable
            {
                TableId = t.Tableid,
                Name = t.TableName,
                Capacity = t.Capacity, 
                Status = t.Status,
                AssignTime = t.CreatedDate ?? DateTime.Now,
            }).ToList()
        };

        Table.Add(kotTableViewModel);
    }


        return Table;
    }

    public async Task<bool> AddWaitingToken(waitingtokenviewmodel model)
     {
        if(model == null)
        {
            return false;
        }
        await _kotTableRepository.AddWaitingToken(model);
        return true;
     }

    public async Task<List<Section>> GetSectionList()
    {
        var sections = await _kotTableRepository.GetSectionList();
        return sections;
    }

    public async Task<List<waitingtokenviewmodel>> GetWaitingList(int id)
    {
        var waitingList = await _kotTableRepository.GetWaitingList(id);
        return waitingList;
    }
    
    public async Task<CustomerDetailsForTableviewmodel> GetCustomerDetails(int id)
    {
        var section = await _kotTableRepository.GetSectionList();

        if(id==0)
        {
            CustomerDetailsForTableviewmodel obj = new CustomerDetailsForTableviewmodel();
            obj.sections = section;
            return obj; 
        }

        var customerDetails = await _kotTableRepository.GetCustomerDetails(id);

        var customer = new CustomerDetailsForTableviewmodel
        {
            Id = (int) customerDetails.Id,
            customerId = (int) customerDetails.Customer.Customerid,
            Name = customerDetails.Customer.Customername,
            Phone = customerDetails.Customer.Phonenumber,
            Email = customerDetails.Customer.Customeremail,
            NoOfPerson =(int) customerDetails.Customer.TotalPersons,
            sectionId = (int) customerDetails.SectionId,
            sectionName = customerDetails.Section.SectionName,
            sections = section,
        };

        return customer;
    }

    public async Task<CustomerDetailsForTableviewmodel> GetCustomerDetailsByEmail(string email)
    {
         var section = await _kotTableRepository.GetSectionList();

        if(email==null)
        {
            CustomerDetailsForTableviewmodel obj = new CustomerDetailsForTableviewmodel();
            obj.sections = section;
            return obj; 
        }

        var customerDetails = await _kotTableRepository.GetCustomerDetailsByEmail(email);

        var customer = new CustomerDetailsForTableviewmodel
        {
            // Id = (int) customerDetails.Id,
            customerId = (int) customerDetails.Customerid,
            Name = customerDetails.Customername,
            Phone = customerDetails.Phonenumber,
            Email = customerDetails.Customeremail,
            NoOfPerson =(int) customerDetails.TotalPersons,
            sectionId = (int) customerDetails.WaitingTokens.FirstOrDefault().SectionId,
            // sectionName = customerDetails.,
            sections = section,
        };

        return customer;
    }
      public async Task<bool> AssignTable(waitingtokenviewmodel model)
      {
        try
        {   
            var customer = await _kotTableRepository.GetCustomerDetails(model.Id);
            var tables = await _kotTableRepository.GetTablesByIdAsync(model.tableId);

          if (customer == null)
          {
            
            var existingcustomer = await _kotTableRepository.GetCustomerFromCustomerTable(model.customerId);

            if(existingcustomer == null)
            {
                var newcustomer = new Customer
                {
                    Customername = model.Name,
                    Customeremail = model.Email,
                    Phonenumber = model.Phone,
                    TotalPersons = model.NoOfPerson,
                    CreatedDate = DateTime.Now,
                };
                await _kotTableRepository.AddCustomer(newcustomer);

                tables.CustomerId = newcustomer.Customerid;
                tables.Status = "Assigned";
                tables.Isavailable = false;

                  await _kotTableRepository.UpdateTables(tables);

               return true;
            }
            else{
                tables.CustomerId = existingcustomer.Customerid;
                tables.Status = "Assigned";
                tables.Isavailable = false;

                  await _kotTableRepository.UpdateTables(tables);

               return true;
            }
              
          }

          else
            {
          tables.CustomerId = model.customerId;
          tables.Status = "Assigned";
          tables.Isavailable = false;
          customer.IsAssigned = true;
          customer.AssignedTime = DateTime.Now;

              await _kotTableRepository.UpdateTables(tables);
             await _kotTableRepository.UpdateCustomer(customer);

                   return true;
            }

        

  
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
         
      }
}
