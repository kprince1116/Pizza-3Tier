namespace BAL.Services;

using System.Collections.Generic;

using BAL.Models.Interfaces;
using DAL.Interfaces;
using Pizzashop.DAL.ViewModels;


public class WaitingService : IWaitingService
{
    private readonly IWaitingRepository _waitingRepository;

    public WaitingService(IWaitingRepository waitingRepository)
    {
        _waitingRepository = waitingRepository;
    }

 
    public async Task<waitingviemodel> GetSections()
    {
        var section =  await _waitingRepository.GetSections();

        var waiting = new waitingviemodel
        {
            sections = section,
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

        waitingToken.Customer.Customeremail = model.Email;
        waitingToken.Customer.Customername = model.Name;
        waitingToken.Customer.Phonenumber = model.Phone;
        waitingToken.NoOfPersons = model.NoOfPerson;
        waitingToken.SectionId = model.sectionId;

        await _waitingRepository.Update(waitingToken); 

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
    
}