using MessagingCorp.BO;
using MessagingCorp.Services.API;
using Microsoft.Extensions.DependencyInjection;
using Ninject;
using SurrealDb;
using SurrealDb.Net;

namespace MessagingCorp.Database
{
    public class DatabaseAccess : IDatabaseAccess
    {
        private SurrealDbClient _client;

        public DatabaseAccess(SurrealDbOptions options)
        {
            _client = new SurrealDbClient(options);
        }

        public void AddUser(string uid, string username, string pass)
        {
            throw new NotImplementedException();
        }

        public bool AuthenticateUser(string uid, string password)
        {
            throw new NotImplementedException();
        }

        public User GetUser(string uid)
        {
            throw new NotImplementedException();
        }

        public bool IsUidExistent(string uid)
        {
            throw new NotImplementedException();
        }

        public void RemoveUser(string uid)
        {
            throw new NotImplementedException();
        }
    }
}
