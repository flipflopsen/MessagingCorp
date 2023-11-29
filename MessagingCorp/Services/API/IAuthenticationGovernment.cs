namespace MessagingCorp.Services.API
{
    public interface IAuthenticationGovernment
    {
        bool AuthenticateUser(string uid, string uniquePassword);
        bool AuthorizeForLobby(string uid, string agreedKey);
    }
}
