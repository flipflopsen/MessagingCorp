using MessagingCorp.BO;
using MessagingCorp.EntityManagement.API;
using Ninject;
using Serilog.Events;
using Serilog;
using System.Net;
using MessagingCorp.Utils.Logger;
using MessagingCorp.Services.Core;
using MessagingCorp.Database.API;
using System.Collections.Concurrent;

namespace MessagingCorp.EntityManagement
{
    public class UserManagement : IUserManagement
    {
        private static readonly ILogger Logger = Log.Logger.ForContextWithConfig<MessageCorpDriver>("./Logs/MessageCorpDriver.log", true, LogEventLevel.Debug);

        private readonly IDatabaseAccess db;
        private readonly ConcurrentDictionary<string, string> UidToSurrealIdUserDict = new ConcurrentDictionary<string, string>();


        [Inject]

        public UserManagement(IDatabaseAccess db) 
        {
            this.db = db;
            PopulateSurrealIdDictFromExistingDb();
        }

        public async Task AddUser(string uid, string username, string password)
        {
            Logger.Information("Adding user: " + uid);
            var surrealId = await db.AddUser(uid, username, password);
            UidToSurrealIdUserDict.TryAdd(uid, surrealId);
        }

        public async Task<User> GetUser(string uid)
        {
            var got = UidToSurrealIdUserDict.TryGetValue(uid, out var surrealId);
            if (got)
                return await db.GetUser(surrealId!);
            else
                return null!;
        }

        public string GetSurrealIdFromUid(string uid)
        {
            var got = UidToSurrealIdUserDict.TryGetValue(uid, out var surrealId);
            if (got)
                return surrealId!;
            else
                return string.Empty!;
        }

        public async Task<bool> RemoveUser(string uid)
        {
            var surrId = GetSurrealIdFromUid(uid);

            if (surrId != string.Empty)
                return await db.RemoveUser(surrId);
            else
                Logger.Warning("[UserManagement] > Attempted to delete user with empty UID, sus!");

            return false;
        }

        private void PopulateSurrealIdDictFromExistingDb()
        {
            var users = db.GetAllUsers().Result;

            if (users != null)
            {
                foreach ( var user in users ) 
                    if (user.Id!.Id != string.Empty && user.UserId != null)
                        UidToSurrealIdUserDict.TryAdd(user.UserId, user.Id!.Id);

                Logger.Information($"[UserManagement] > Populated {users.Count()} Users from existing db!");
            }
                
        }
    }
}
