using MessagingCorp.Utils;

namespace MessagingCorp.BO.BusMessages
{
    public class CorpMessage
    {
        public string OriginatorUserId { get; set; } = string.Empty;
        public string TargetUserId { get; set; } = string.Empty;

        public Utils.Action Action { get; set; }
        public bool IsSymm {  get; set; }
        public bool IsAsymm {  get; set; }

        public string Password { get; set; } = string.Empty;
        public string Challenge { get; set; } = string.Empty;

        public string LobbyId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public string AdditionalData { get; set; } = string.Empty;
    }
}
