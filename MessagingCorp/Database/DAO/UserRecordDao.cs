using SurrealDb.Net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Database.DAO
{
    public class UserRecordDao : Record
    {
        public string UserId { get; set; }
        public string UserName { get; set;}
        public string Password { get; set;}

        public List<string> ActiveLobbyParticipations { get; set; }

        public List<string> FriendList { get; set; }

        public List<string> PendingFriendRequests { get; set; }

        public UserRecordDao(string userId, string userName, string password, List<string> activeLobbyParticipations = null!, List<string> friendList = null!, List<string> pendingFriendRequests = null!)
        {
            UserId = userId;
            UserName = userName;
            Password = password;
            ActiveLobbyParticipations = activeLobbyParticipations ?? new List<string>();
            FriendList = friendList ?? new List<string>();
            PendingFriendRequests = pendingFriendRequests ?? new List<string>();
        }
    }
}
