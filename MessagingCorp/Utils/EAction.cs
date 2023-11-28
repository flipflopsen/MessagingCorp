namespace MessagingCorp.Utils
{
    public enum Action
    {
        Invalid,

        // User Management
        RegisterUser,
        LoginUser,

        // Lobby Management
        CreateLobby,
        JoinLobby,
        DeleteLobby,

        // Crypto Management
        ChangeSymmetric,
        ChangeAsymmetric,
        UpdateSymmetric,
        UpdateAsymmetric,

        // User Actions
        SendMessage,
        UpdateMessage,
        PurgeUser
    }
}
