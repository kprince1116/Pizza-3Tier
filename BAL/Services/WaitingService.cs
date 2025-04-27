namespace BAL.Services;

using System.Collections.Generic;

using BAL.Models.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pizzashop.DAL.ViewModels;


public class WaitingService : IWaitingService
{
    private readonly IWaitingRepository _waitingRepository;

    private readonly IKotTableRepository _kotTableRepository;

    public WaitingService(IWaitingRepository waitingRepository , IKotTableRepository kotTableRepository)
    {
        _waitingRepository = waitingRepository;
        _kotTableRepository = kotTableRepository;
    }

 
    public async Task<waitingviemodel> GetSections()
    {
        var section =  await _waitingRepository.GetSections();

        var waiting = new waitingviemodel
        {
            sections = section,
            TotalWaiting = section.Sum(s=>s.WaitingTokens.Count(u=>u.IsDeleted == false && u.IsAssigned == false)),
        };

        return waiting;
    }

    public async Task<waitingviemodel> GetWaitingList(int sectionId)
    {
        var waitingList = await _waitingRepository.GetWaitingList(sectionId);

        var waiting = new waitingviemodel
        {
            waiting = waitingList,
        };

        return waiting;
    }

     public async Task<waitingtokenviewmodel> GetCustomerDetailsByEmail(int sectionid , string email)
    {
         var section = await _waitingRepository.GetSections();

        if(email==null)
        {
            waitingtokenviewmodel obj = new waitingtokenviewmodel();
            obj.sections = section;
            return obj; 
        }

        var customerDetails = await _waitingRepository.GetCustomerDetailsByEmail(email);

        var customer = new waitingtokenviewmodel
        {
            // Id = (int) customerDetails.Id,
            customerId = (int) customerDetails.Customerid,
            Name = customerDetails.Customername,
            Phone = customerDetails.Phonenumber,
            Email = customerDetails.Customeremail,
            NoOfPerson =(int) customerDetails.TotalPersons,
            sectionId = (int) sectionid,
            // sectionName = customerDetails.,
            sections = section,
        };

        return customer;
    }

    public async Task<waitingtokenviewmodel> EditToken(int id)
    {
        var waitingList = await _waitingRepository.EditToken(id);
        
        return waitingList;
    }

    public async Task<bool> UpdateWaitingToken(waitingtokenviewmodel model)
    {
       try
       {
          var waitingToken = await _waitingRepository.GetTokenId(model.Id);

        if (waitingToken == null)
        {
            return false;
        }

        waitingToken.Customer.Customername = model.Name;
        waitingToken.Customer.Phonenumber = model.Phone;
        waitingToken.NoOfPersons = model.NoOfPerson;
        waitingToken.SectionId = model.sectionId;

        await _waitingRepository.Update(waitingToken); 

        var customer = await _waitingRepository.GetCustomer(model.customerId);

        {
            if (customer == null)
            {
                return false;
            }

            customer.Customername = model.Name;
            customer.Phonenumber = model.Phone;
            customer.TotalPersons = model.NoOfPerson;

            await _waitingRepository.UpdateCustomers(customer);
        }

        return true;
       }
       catch (Exception e)
       {
        Console.WriteLine(e);
        return false;
       }
      
        }

    public async Task<bool> DeleteToken(int id)
    {
        var waitingtoken = await _waitingRepository.GetTokenId(id);

        if (waitingtoken == null)
        {
            return false;
        }

        waitingtoken.IsDeleted = true;

        await _waitingRepository.Update(waitingtoken);

        return true;
    }

    public async Task<waitingviemodel> GetTableDetails()
    {
        var Sections = await _waitingRepository.GetTableDetails();
    
        var Tables = new List<waitingviemodel>();
    
        foreach(var section in Sections)
        {
            var tables= await _waitingRepository.GetTablesBySectionId(section.Sectionid);
    
              var waitingTable = new waitingviemodel
                {
                   AvailableTables = tables.Select(t=> new WaitingTable
                   {
                       sectionId = section.Sectionid,
                       TableId = t.Tableid,
                       Name = t.TableName,
                   }).ToList(),
                };
    
                Tables.Add(waitingTable);
            
        } 
        
        var table = new waitingviemodel
        {
            sections = Sections,
            AvailableTables = Tables.SelectMany(t => t.AvailableTables).ToList()
        };
    
        return table;
    }

    public async Task<int> AssignTable(waitingtokenviewmodel model)
    {
        var customer = await _waitingRepository.GetCustomerById(model.Id);
        var tables = await _waitingRepository.GetTableBySectionId(model.tableId);
       
       if(customer == null)
       {
        return 0;
       }

       tables.CustomerId = model.customerId;
       tables.Status = "Assigned";
       tables.Isavailable = false;
       customer.IsAssigned = true;
       customer.AssignedTime = DateTime.Now;

       await _waitingRepository.UpdateTable(tables);
       await _waitingRepository.UpdateCustomer(customer);

        Order order = new()
            {
                CustomerId =(int) model.customerId,
                CreatedDate = DateTime.Now,
                Orderdate = DateTime.Now,
                Status = 1,
                NoOfPerson = model.NoOfPerson
            };
 
            order = await _kotTableRepository.GenerateOrder(order);

            OrderTable orderTable = new(){
                OrderId = order.Orderid,
                TableId = tables.Tableid,
                CustomerId = model.customerId 
            };

            await _kotTableRepository.AddOrderTable(orderTable);


        return order.Orderid;
    }
    
}