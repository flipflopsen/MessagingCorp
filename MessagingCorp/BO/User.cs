using Castle.Core.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.BO
{
    public class User
    {
        // todo: make fields private and utilize automapper
        public List<string> ActiveLobbyParticipations { get; set; }
        public List<string> FriendList { get; set; }
        public List<string> PendingFriendRequests { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? SurrealId { get; set; }

        public User() 
        { 
            // Default constructor for conversion
        }

        public User(string userId, string userName, string password, List<string> activeLobbyParticipations = null!, List<string> friendList = null!, List<string> pendingFriendRequests = null!)
        {
            UserId = userId;
            UserName = userName;
            Password = password;
            ActiveLobbyParticipations = activeLobbyParticipations ?? new List<string>();
            FriendList = friendList ?? new List<string>();
            PendingFriendRequests = pendingFriendRequests ?? new List<string>();
        }

        public bool IsUserInLobby(string lobbyId) 
        {
            return ActiveLobbyParticipations.Contains(lobbyId);
        }

        public void AddLobbyParticipation(string lobbyId)
        {
            if (ActiveLobbyParticipations.Contains(lobbyId))
                return;
            ActiveLobbyParticipations.Add(lobbyId);
        }

        public void RemoveLobbyParticipation(string lobbyId)
        {
            if (!ActiveLobbyParticipations.Contains(lobbyId))
                return;
            ActiveLobbyParticipations.Remove(lobbyId);
        }

        public bool ValidateUserIntegrity()
        {
            return !(UserId.IsNullOrEmpty() && UserName.IsNullOrEmpty() && Password.IsNullOrEmpty());
        }

        public void AddFriend(string friendId)
        {
            if (FriendList.Contains(friendId)) 
                return; 
            FriendList.Add(friendId);
        }

        public void RemoveFriend(string friendId)
        {
            if (!FriendList.Contains(friendId))
                return;
            FriendList.Remove(friendId);
        }
    }
}
