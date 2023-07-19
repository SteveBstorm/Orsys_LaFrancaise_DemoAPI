using DemoWebAPI.Entities;
using Microsoft.AspNetCore.SignalR;

namespace DemoWebAPI.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(Message m)
        {
            await Clients.All.SendAsync("RecieveMessage", m);
        }
    }
}
