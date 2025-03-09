using Microsoft.AspNetCore.SignalR;

namespace Lab9.Chat;

public class ChatHub : Hub
{
    public async Task JoinChat(string userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, userId);
    }

    public async Task SendMessage(string senderId, string receiverId, string message)
    {
        await Clients.Group(receiverId).SendAsync("ReceiveMessage", senderId, message);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, Context.UserIdentifier);
        await base.OnDisconnectedAsync(exception);
    }
}

