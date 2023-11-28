namespace MessagingCorp.Utils.Converters
{
    public static class ActionToEnumConverter
    {
        public static Action ConvertToAction(string action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            switch (action)
            {
                case "login": return Action.LoginUser;
                case "register": return Action.RegisterUser;
                case "createlobby": return Action.CreateLobby;
                case "joinlobby": return Action.JoinLobby;
                case "deletelobby": return Action.DeleteLobby;
                case "changesym": return Action.ChangeSymmetric;
                case "changeasym": return Action.ChangeAsymmetric;
                case "updsym": return Action.UpdateSymmetric;
                case "updasym": return Action.UpdateAsymmetric;
                case "sendmsg": return Action.SendMessage;
                case "updmsg": return Action.UpdateMessage;
                case "purge": return Action.PurgeUser;
                default: return Action.Invalid;
            }
        }
    }
}
