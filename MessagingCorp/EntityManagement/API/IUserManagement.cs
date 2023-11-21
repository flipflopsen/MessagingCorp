using MessagingCorp.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.EntityManagement.API
{
    public interface IUserManagement
    {
        void AddUser(string uid, string password);
        void RemoveUser(string uid);
        User GetUser(string uid);
    }
}
