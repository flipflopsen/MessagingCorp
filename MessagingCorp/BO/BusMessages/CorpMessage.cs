using MessagingCorp.Common.Enumeration;
using MessagingCorp.Crypto.Symmetric;
using System.Net;

namespace MessagingCorp.BO.BusMessages
{
    public class CorpMessage
    {
        public string OriginatorUserId { get; set; } = string.Empty;
        public string OriginatorUserName { get; set; } = string.Empty;
        public string TargetUserId { get; set; } = string.Empty;

        public CorpUserAction Action { get; set; }
        public bool IsSymm {  get; set; }
        public bool IsAsymm {  get; set; }

        public EEncryptionStrategySymmetric SymEncStrat {  get; set; }
        public EEncryptionStrategyAsymmetric AsyncEncStrat { get; set; }

        public string Password { get; set; } = string.Empty;
        public string Challenge { get; set; } = string.Empty;

        public string LobbyId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public string AdditionalData { get; set; } = string.Empty;

        public HttpListenerContext HttpContext { get; set; }
    }
}
