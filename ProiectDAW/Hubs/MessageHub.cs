using Microsoft.AspNetCore.SignalR;

namespace ProiectDAW.Hubs
{
    public class MessageHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessageHandler", user, message);
        }
    }
}
