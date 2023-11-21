using MessagingCorp.BO;
using MessagingCorp.EntityManagement.API;
using MessagingCorp.Services;
using MessagingCorp.Services.API;
using Ninject;
using Serilog.Events;
using Serilog;
using System.Net;
using MessagingCorp.Utils.Logger;

namespace MessagingCorp.EntityManagement
{
    public class UserManagement : IUserManagement
    {
        private static readonly ILogger Logger = Log.Logger.ForContextWithConfig<MessageCorpDriver>("./Logs/MessageCorpDriver.log", true, LogEventLevel.Debug);
        private readonly IDatabaseAccess db;

        public UserManagement(IKernel kernel) 
        { 
            db = kernel.Get<IDatabaseAccess>();
        }

        public void AddUser(string uid, string password)
        {
            Logger.Information("Adding user: " + uid);
            db.AddUser(uid, password);
        }

        public User GetUser(string uid)
        {
            return db.GetUser(uid);
        }

        public void RemoveUser(string uid)
        {
            db.RemoveUser(uid);
        }
    }
}
