using MessagingCorp.Common.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.EntityManagement.BO
{
    public record struct FriendRequest(string TargetUid, CorpActionDirection Direction);
    public record struct PendingMessage(string OriginatorUid, string EncryptedMessage);
}
