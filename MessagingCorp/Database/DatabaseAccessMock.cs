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
        private readonly Dictionary<string, User> users = new Dictionary<string, User>();

        public DatabaseAccessMock() { }

        public void AddUser(string uid, string username, string pass)
        {
            users.Add(uid, new User(uid, username, pass, 0));
        }

        public void RemoveUser(string uid) 
        {
            users.Remove(uid);
        }
        public User GetUser(string uid)
        {
            return users[uid];
        }
        public bool AuthenticateUser(string uid, string password)
        {
            if (!users.ContainsKey(uid))
                return false;
            return users[uid].Password! == password;
        }

        public bool IsUidExistent(string uid)
        {
            return users.ContainsKey(uid);
        }
    }
}
