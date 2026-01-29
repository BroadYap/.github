using BroadYap.DataService;
using BroadYap.Models;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace BroadYap.Hubs
{
    public class ChatHub : Hub
    {
        private readonly SharedDb _sharedDb;

        public ChatHub(SharedDb sharedDb)
        {
            _sharedDb = sharedDb;
        }

        public async Task JoinChatRoom(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                await Clients.Caller.SendAsync("ReceiveMessage", "admin", "Username cannot be empty.");
                return;
            }

            string chatRoom = "General";
            await Groups.AddToGroupAsync(Context.ConnectionId, chatRoom);
            _sharedDb.Connection[Context.ConnectionId] = new UserConnection { UserName = userName, ChatRoom = chatRoom };

            await Clients.Group(chatRoom).SendAsync("ReceiveMessage", "admin", $"{userName} has joined the chat room {chatRoom}");
        }

        public async Task SendMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                await Clients.Caller.SendAsync("ReceiveMessage", "admin", "Message cannot be empty.");
                return;
            }

            var ConnectionId = Context.ConnectionId;

            var ConnectionUser = _sharedDb.Connection[ConnectionId];

            string userName = ConnectionUser.UserName;

            var MessageData = new
            {
                User = userName,
                Text = message,
                Timestamp = System.DateTime.UtcNow
            };

            await Clients.Group(ConnectionUser.ChatRoom).SendAsync("ReceiveMessage", MessageData);
        }
    }
}