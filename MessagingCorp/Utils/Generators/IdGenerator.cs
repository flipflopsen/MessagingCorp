namespace MessagingCorp.Utils
{
    public static class IdGenerator
    {
        private const string PREFIX = "GrownCorp-";
        private const string USER_PREFIX = "User-";
        private const string LOBBY_PREFIX = "Lobby-";

        public static string GenerateNewUserUid()
        {
            return PREFIX + USER_PREFIX + Guid.NewGuid().ToString("N").Substring(0, 12).Replace("-", "");
        }

        public static string GenerateNewLobbyId()
        {
            return PREFIX + LOBBY_PREFIX + Guid.NewGuid().ToString("N").Substring(0, 12).Replace("-", "");
        }
    }
}
