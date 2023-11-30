namespace MessagingCorp.Services.API
{
    public interface IAuthenticationGovernment
    {
        Task<bool> AuthenticateUser(string uid, string uniquePassword);
        Task<bool> AuthorizeForLobby(string uid, string agreedKey);
    }
}
