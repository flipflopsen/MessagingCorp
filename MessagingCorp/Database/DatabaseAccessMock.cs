using MessagingCorp.BO;
using MessagingCorp.Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Database
{
    public class DatabaseAccessMock : IDatabaseAccess
    {
        private readonly Dictionary<string, string> users = new Dictionary<string, string>()
        {
            { "123", "321" }
        };

        public DatabaseAccessMock() { }

        public void AddUser(string uid, string pass)
        {
            users.Add(uid, pass);
        }

        public void RemoveUser(string uid) 
        {
            users.Remove(uid);
        }
        public User GetUser(string uid)
        {
            return new User(uid, users[uid], 123);
        }
        public bool AuthenticateUser(string uid, string password)
        {
            if (!users.ContainsKey(uid))
                return false;
            return users[uid] == password;
        }

        public bool IsUidExistent(string uid)
        {
            return true;
        }
    }
}
