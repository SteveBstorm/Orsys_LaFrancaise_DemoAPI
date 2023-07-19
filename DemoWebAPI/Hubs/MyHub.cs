using Microsoft.AspNetCore.SignalR;

namespace DemoWebAPI.Hubs
{
    public class MyHub : Hub
    {
        public async Task NotifyArticleUpdate()
        {
            await Clients.All.SendAsync("ArticleUpdate");
        }
    }
}
