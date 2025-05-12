using Microsoft.AspNetCore.SignalR;

public class NotificationHub : Hub
{
    public async Task SendMessage( string message)
    {
        await Clients.All.SendAsync("TableMessage",  message);
        await Clients.All.SendAsync("TaxMessage",  message);
        await Clients.All.SendAsync("KotMessage",  message);
        await Clients.All.SendAsync("WaitingMessage",  message);
        await Clients.All.SendAsync("KotTableMessage",  message);
        await Clients.All.SendAsync("UserMessage",  message);
        await Clients.All.SendAsync("ItemMessage",  message);
        await Clients.All.SendAsync("ModifierMessage",  message);
        await Clients.All.SendAsync("KotUpdatedMessage",  message);
        await Clients.All.SendAsync("OrderUpdatedMessage",  message);
    }
}