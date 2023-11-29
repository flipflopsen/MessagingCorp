using MessagingCorp.Utils.Enumeration;

namespace MessagingCorp.Utils.Converters
{
    public static class ActionToEnumConverter
    {
        public static CorpUserAction ConvertToAction(string action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            switch (action)
            {
                case "login": return CorpUserAction.LoginUser;
                case "register": return CorpUserAction.RegisterUser;
                case "createlobby": return CorpUserAction.CreateLobby;
                case "joinlobby": return CorpUserAction.JoinLobby;
                case "deletelobby": return CorpUserAction.DeleteLobby;
                case "changesym": return CorpUserAction.ChangeSymmetric;
                case "changeasym": return CorpUserAction.ChangeAsymmetric;
                case "updsym": return CorpUserAction.UpdateSymmetric;
                case "updasym": return CorpUserAction.UpdateAsymmetric;
                case "sendmsg": return CorpUserAction.SendMessage;
                case "updmsg": return CorpUserAction.UpdateMessage;
                case "purge": return CorpUserAction.PurgeUser;
                default: return CorpUserAction.Invalid;
            }
        }
    }
}
