using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Services.API
{
    public interface IAuthenticationGovernment
    {
        bool AuthenticateUser(string uid, string uniquePassword);
        bool AuthorizeForLobby(string uid, string agreedKey);
    }
}
