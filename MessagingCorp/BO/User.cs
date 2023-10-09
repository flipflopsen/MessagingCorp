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
        private string _userId;
        public string DisplayedName { get; set; }

        private readonly List<string> _activeLobbyParticipations;
        private readonly string? _password;

        public User(string wantedNickname, string? password, int? pin)
        {
            DisplayedName = wantedNickname;
            _activeLobbyParticipations = new List<string>();
        }

        public void SetUserId(string userId)
        {
            this._userId = userId;
        }

        public bool ValidateUserIntegrity()
        {
            return !(_userId.IsNullOrEmpty() && DisplayedName.IsNullOrEmpty() && _password.IsNullOrEmpty());
        }
    }
}
