using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Pizzashop.DAL.ViewModels;

namespace DAL.Repository;

public class WaitingRepository : IWaitingRepository
{
    private readonly PizzaShopContext _db;

    public WaitingRepository(PizzaShopContext db)
    {
        _db = db;
    }

    public async Task<List<Section>> GetSections()
    {
        return await _db.Sections.Include(s => s.WaitingTokens.Where(w => w.IsDeleted == false && w.IsAssigned == false)).Where(u=>u.Isdeleted == false).OrderBy(u=>u.Sectionid).ToListAsync();
    }

    public async Task<List<waitingtokenviewmodel>> GetWaitingList(int sectionId)
    {
        var waitingList = await _db.WaitingTokens.Include(u=>u.Customer).Include(u=>u.Section).Where(u=>u.IsDeleted==false && u.IsAssigned==false).
                                                    Select(u=> new waitingtokenviewmodel
                                                    {
                                                        Id = u.Id,
                                                        Email = u.Customer.Customeremail,
                                                        Name = u.Customer.Customername,
                                                        Phone = u.Customer.Phonenumber,
                                                        CreatedAt = u.CreatedDate.GetValueOrDefault(),
                                                        NoOfPerson =(int) u.NoOfPersons,
                                                        sectionId = u.Section.Sectionid
                                                    }).OrderBy(u=>u.Id).ToListAsync();
    
        if(sectionId != 0)
        {
            waitingList =  waitingList.Where(v => v.sectionId == sectionId).ToList();
        }
        
    
        return waitingList;
    }

    public async Task<waitingtokenviewmodel> EditToken(int id)
    {
        var waiting = await _db.WaitingTokens.Include(u=>u.Customer).Include(u=>u.Section).Where(u => u.Id == id && u.IsDeleted == false)
                        .Select(u => new waitingtokenviewmodel
                        {
                            Id = u.Id,
                            Email = u.Customer.Customeremail,
                            Name = u.Customer.Customername,
                            Phone = u.Customer.Phonenumber,
                            CreatedAt = u.CreatedDate.GetValueOrDefault(),
                            NoOfPerson = (int)u.NoOfPersons,
                            sectionId = u.Section.Sectionid,
                            sections = _db.Sections.Where(u => u.Isdeleted == false).ToList()
                        })
                        .FirstOrDefaultAsync();
        return waiting;
    }

    public async Task<WaitingToken> GetTokenId(int Id)
    {
        return await _db.WaitingTokens.Include(u=>u.Customer).Include(u=>u.Section).FirstOrDefaultAsync(u => u.Id == Id);
    }

    public async Task Update(WaitingToken waitingToken)
    {
        _db.WaitingTokens.Update(waitingToken);
        await _db.SaveChangesAsync();
    }

}
