using MessagingCorp.Common.Enumeration;
using MessagingCorp.Common.Logger;
using MessagingCorp.Database.API;
using MessagingCorp.EntityManagement.API;
using MessagingCorp.EntityManagement.BO;
using MessagingCorp.Services.Core;
using Ninject;
using Serilog;
using Serilog.Events;
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

        #region Add/Get/Remove User

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

        public async Task<bool> RemoveUser(string uid)
        {
            var surrId = GetSurrealIdFromUid(uid);

            if (surrId != string.Empty)
                return await db.RemoveUser(surrId);
            else
                Logger.Warning("[UserManagement] > Attempted to delete user with empty UID, sus!");

            return false;
        }

        #endregion

        #region FriendList Logic
        public async Task SendFriendRequest(string originatorUid, string targetUid)
        {
            var originator = await GetUser(originatorUid);
            var target = await GetUser(targetUid);

            if (originator != null && target != null)
            {
                target.AddFriendRequest(new FriendRequest(originatorUid, CorpActionDirection.Incoming));
                originator.AddFriendRequest(new FriendRequest(targetUid, CorpActionDirection.Outgoing));

                await db.UpdateUser(originator!);
                await db.UpdateUser(target!);
                Logger.Information($"[UserManagement] > {originator!.UserName} ({originatorUid}) sent a friend request to {target!.UserName} ({targetUid}).");
            }

        }

        public async Task AcceptFriendRequest(string originatorUid, string targetUid)
        {
            var originator = await GetUser(originatorUid);
            var target = await GetUser(targetUid);

            if (target != null && originator != null)
            {
                target.AddFriend(originatorUid);
                originator.AddFriend(targetUid);

                await db.UpdateUser(originator!);
                await db.UpdateUser(target!);
                Logger.Information($"[UserManagement] > {originator!.UserName} ({originatorUid}) accepted the friend request of {target!.UserName} ({targetUid}), they are friends now!");
            }
        }


        #endregion

        #region SurrealDB Helpers
        public string GetSurrealIdFromUid(string uid)
        {
            var got = UidToSurrealIdUserDict.TryGetValue(uid, out var surrealId);
            if (got)
                return surrealId!;
            else
                return string.Empty!;
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
        #endregion
    }
}
