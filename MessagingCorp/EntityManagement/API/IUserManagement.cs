using MessagingCorp.EntityManagement.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.EntityManagement.API
{
    public interface IUserManagement
    {
        Task AddUser(string uid, string username, string password);
        Task<bool> RemoveUser(string uid);
        Task<User> GetUser(string uid);

        string GetSurrealIdFromUid(string uid);

        Task SendFriendRequest(string originatorUid, string targetUid);
        Task AcceptFriendRequest(string originatorUid, string targetUid);

    }
}
