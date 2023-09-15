using MessagingCorp.Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Auth
{
    public class BaseAuthenticator : IAuthenticationGovernment
    {
        public bool AuthenticateUser(string uid, string uniquePassword)
        {
            throw new NotImplementedException();
        }

        public bool AuthorizeForLobby(string uid, string agreedKey)
        {
            throw new NotImplementedException();
        }

        public string CreateNewUser(string uniquePassword)
        {
            throw new NotImplementedException();
        }

        public bool RemoveUser(string uniquePassword)
        {
            throw new NotImplementedException();
        }
    }
}
