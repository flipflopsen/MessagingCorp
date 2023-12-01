using Castle.Core.Internal;
using SurrealDb.Net;
using SurrealDb.Net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
/*
namespace MessagingCorp.EntityManagement.BO
{
    public class User
    {
        // todo: make fields private and utilize automapper
        public List<string> ActiveLobbyParticipations { get; set; }
        public List<string> FriendList { get; set; }
        public List<FriendRequest> PendingFriendRequests { get; set; }
        public List<PendingMessage> PendingMessages { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Thing SurrealId { get; set; }

        public User()
        {
            // Default constructor for conversion
        }

        public User(string userId, string userName, string password, List<string> activeLobbyParticipations = null!, List<string> friendList = null!, List<FriendRequest> pendingFriendRequests = null!)
        {
            UserId = userId;
            UserName = userName;
            Password = password;
            ActiveLobbyParticipations = activeLobbyParticipations ?? new List<string>();
            FriendList = friendList ?? new List<string>();
            PendingFriendRequests = pendingFriendRequests ?? new List<FriendRequest>();
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

            if (PendingFriendRequests.Exists(r => r.TargetUid == friendId))
            {
                PendingFriendRequests.Remove(PendingFriendRequests.First(r => r.TargetUid == friendId));
                FriendList.Add(friendId);
            }
        }

        public void RemoveFriend(string friendId)
        {
            if (!FriendList.Contains(friendId))
                return;
            FriendList.Remove(friendId);
        }

        public void AddFriendRequest(FriendRequest friendId)
        {
            if (PendingFriendRequests.Contains(friendId))
                return;
            PendingFriendRequests.Add(friendId);
        }

        public void AddPendingMessage(PendingMessage message)
        {
            PendingMessages.Add(message);
        }

        public List<PendingMessage> GetPendingMessages() 
        { 
            return PendingMessages; 
        }
    }
}
*/
using AutoMapper;
using MessagingCorp.Database.DAO;

namespace MessagingCorp.EntityManagement.BO
{
    public class User
    {
        public List<string> ActiveLobbyParticipations { get; set; }
        public List<string> FriendList { get; set; }
        public List<FriendRequest> PendingFriendRequests { get; set; }
        public List<PendingMessage> PendingMessages { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Thing SurrealId { get; set; }

        //public IReadOnlyList<string> ActiveLobbyParticipations => _activeLobbyParticipations;
        //public IReadOnlyList<string> FriendList => _friendList;
       // public IReadOnlyList<FriendRequest> PendingFriendRequests => _pendingFriendRequests;
        //public IReadOnlyList<PendingMessage> PendingMessages => _pendingMessages;

        // ... rest of the code

        public User() { } // Default constructor for AutoMapper

        public User(string userId, string userName, string password, List<string> activeLobbyParticipations = null, List<string> friendList = null, List<FriendRequest> pendingFriendRequests = null)
        {
            UserId = userId;
            UserName = userName;
            Password = password;
            ActiveLobbyParticipations = activeLobbyParticipations ?? new List<string>();
            FriendList = friendList ?? new List<string>();
            PendingFriendRequests = pendingFriendRequests ?? new List<FriendRequest>();
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
            if (PendingFriendRequests.Exists(r => r.TargetUid == friendId))
            {
                PendingFriendRequests.Remove(PendingFriendRequests.First(r => r.TargetUid == friendId));

                if (FriendList.Contains(friendId))
                    return;

                FriendList.Add(friendId);
            }
        }

        public void RemoveFriend(string friendId)
        {
            if (!FriendList.Contains(friendId))
                return;
            FriendList.Remove(friendId);
        }

        public void AddFriendRequest(FriendRequest friendId)
        {
            if (FriendList.Contains(friendId.TargetUid)) 
                return;
            if (PendingFriendRequests.Contains(friendId))
                return;
            PendingFriendRequests.Add(friendId);
        }

        public void AddPendingMessage(PendingMessage message)
        {
            PendingMessages.Add(message);
        }

        public List<PendingMessage> GetPendingMessages()
        {
            return PendingMessages;
        }

        public void SetSurrealId(Thing id)
        {
            SurrealId = id;
        }

        public class UserProfile : Profile
        {
            public UserProfile()
            {
                CreateMap<UserRecordDao, User>();
                CreateMap<User, UserRecordDao>();
            }
        }
    }
}