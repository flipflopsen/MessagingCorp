using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Services.API
{
    public interface IAuthenticationGovernment
    {
        string CreateNewUser(string uniquePassword);
        bool AuthenticateUser(string uid, string uniquePassword);
        bool AuthorizeForLobby(string uid, string agreedKey);
        bool RemoveUser(string uniquePassword);
    }
}
