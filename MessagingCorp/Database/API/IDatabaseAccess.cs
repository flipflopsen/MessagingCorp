using MessagingCorp.BO;
using MessagingCorp.Database.DAO;

namespace MessagingCorp.Database.API
{
    public interface IDatabaseAccess
    {
        Task<bool> IsUidExistent(string uid);
        Task<bool> AuthenticateUser(string uid, string password);

        Task<string> AddUser(string uid, string username, string pass);

        Task<bool> RemoveUser(string uid);

        Task<User> GetUser(string uid);

        Task<IEnumerable<UserRecordDao>> GetAllUsers();
    }
}
