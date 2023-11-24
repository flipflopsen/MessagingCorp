using Castle.Core.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.BO
{
    public class User
    {
        // todo: profilbild

        private string _userId;
        public string DisplayedName { get; set; }

        private readonly List<string> _activeLobbyParticipations;
        public string? Password;

        public User(string uid, string wantedNickname, string? password, int? pin)
        {
            _userId = uid;
            DisplayedName = wantedNickname;
            _activeLobbyParticipations = new List<string>();
        }

        public void SetUserId(string userId)
        {
            this._userId = userId;
        }

        public bool ValidateUserIntegrity()
        {
            return !(_userId.IsNullOrEmpty() && DisplayedName.IsNullOrEmpty() && Password.IsNullOrEmpty());
        }
    }
}
