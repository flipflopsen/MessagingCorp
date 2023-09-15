using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Services.API
{
    public interface IUserManagement
    {
        string CreateUser(string username, string password);
        void GetUser(string uid);
    }
}
