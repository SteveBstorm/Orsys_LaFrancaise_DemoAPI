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

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await SendToGroup(groupName, new Message
            {
                Username = "System",
                Content = "Un nouvel utilisateur est arrivé",
                SendDate = DateTime.Now
            });
        }

        public async Task SendToGroup(string groupName, Message m)
        {
            await Clients.Group(groupName).SendAsync("RecieveFromGroup", m);
        }
    }
}
