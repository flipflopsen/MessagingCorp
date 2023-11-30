namespace MessagingCorp.Services.API
{
    public interface IAuthenticationGovernment
    {
        Task<bool> AuthenticateUser(string uid, string uniquePassword);
        Task<bool> IsUserAuthenticated(string uid);
        Task<bool> AuthorizeForLobby(string uid, string agreedKey);
        Task<bool> IsUserAuthorizedForLobby(string uid, string lobbyId);
    }
}
