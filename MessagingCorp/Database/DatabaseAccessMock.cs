using MessagingCorp.BO;
using MessagingCorp.Database.API;
using MessagingCorp.Database.DAO;
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

        public async Task<string> AddUser(string uid, string username, string pass)
        {
            users.Add(uid, new User(uid, username, pass));
            return string.Empty;
        }

        public async Task<bool> RemoveUser(string uid) 
        {
            users.Remove(uid);
            return true;
        }
        public Task<User> GetUser(string uid)
        {
            return new Task<User>(() => users[uid]);
        }
        public Task<bool> AuthenticateUser(string uid, string password)
        {
            if (!users.ContainsKey(uid))
                return new Task<bool>(() => false);
            if (users[uid].Password! == password)
                return new Task<bool>(() => true);
            return new Task<bool>(() => false);
        }

        public Task<bool> IsUidExistent(string uid)
        {
            return new Task<bool>(() => true);
        }

        public Task<IEnumerable<UserRecordDao>> GetAllUsers()
        {
            throw new NotImplementedException();
        }
    }
}
