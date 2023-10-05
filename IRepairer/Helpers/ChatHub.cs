using Microsoft.AspNetCore.SignalR;

namespace IRepairer.Helpers;

public class ChatHub : Hub
{
    public override async Task OnConnectedAsync() => await Clients.All.SendAsync("ReceiveMessage", $"{Context?.ConnectionId} has joined");
    public async Task SendMessage(string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
        await Console.Out.WriteLineAsync(message);
    }
}
