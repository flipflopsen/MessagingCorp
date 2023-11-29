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
        // todo: profilbild

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public List<string> ActiveLobbyParticipations { get; set; }

        //private readonly List<string> _activeLobbyParticipations;

        public User() { }

        public User(string uid, string nickname, string password)
        {
            UserId = uid;
            UserName = nickname;
            //_activeLobbyParticipations = new List<string>();
            ActiveLobbyParticipations = new List<string>();
            Password = password;
        }

        public User(string uid, string nickname, string password, List<string> activeLobbyParticipations)
        {
            UserId = uid;
            UserName = nickname;
            //_activeLobbyParticipations = new List<string>();
            ActiveLobbyParticipations = activeLobbyParticipations;
            Password = password;
        }

        public bool IsUserInLobby(string lobbyId) 
        {
            //return _activeLobbyParticipations.Contains(lobbyId);
            return ActiveLobbyParticipations.Contains(lobbyId);
        }

        public bool ValidateUserIntegrity()
        {
            return !(UserId.IsNullOrEmpty() && UserName.IsNullOrEmpty() && Password.IsNullOrEmpty());
        }
    }
}
