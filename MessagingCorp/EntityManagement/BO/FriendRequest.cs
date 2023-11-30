using MessagingCorp.Common.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.EntityManagement.BO
{
    public class FriendRequest
    {
        public CorpActionDirection Direction { get; set; }
        public string TargetUid { get; set; }

        public FriendRequest(string targetUid, CorpActionDirection direction) 
        {
            TargetUid = targetUid;
            Direction = direction;
        }

    }
}
