using MessagingCorp.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Services.API
{
    public interface IDatabaseAccess
    {
        Task<bool> IsUidExistent(string uid);
        Task<bool> AuthenticateUser(string uid, string password);

        Task AddUser(string uid, string username, string pass);

        Task RemoveUser(string uid);

        Task<User> GetUser(string uid);
    }
}
