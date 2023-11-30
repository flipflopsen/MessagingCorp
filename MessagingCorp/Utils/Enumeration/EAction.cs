﻿namespace MessagingCorp.Utils.Enumeration
{
    public enum CorpUserAction
    {
        Invalid,

        // User Management
        RegisterUser,
        LoginUser,
        AddFriendRequest,
        AcceptFriendRequest,

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
