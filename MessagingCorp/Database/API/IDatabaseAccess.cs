using MessagingCorp.Database.DAO;
using MessagingCorp.EntityManagement.BO;

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

        Task<bool> UpdateUser(User user);
    }
}
