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
        bool IsUidExistent(string uid);
        bool AuthenticateUser(string uid, string password);

        public void AddUser(string uid, string username, string pass);

        public void RemoveUser(string uid);

        public User GetUser(string uid);
    }
}
