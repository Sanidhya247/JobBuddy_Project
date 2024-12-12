using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace job_buddy_backend.Core.ChatService
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(int chatId, int senderId, string message)
        {
            await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", senderId, message);
        }

        public async Task JoinChatGroup(int chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        }

        public async Task LeaveChatGroup(int chatId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
        }
    }
}
