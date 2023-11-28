using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Configuration.BO
{
    public class EncryptionConfiguration : BaseConfiguration
    {
        public string RequestSecurityChallenge { get; set; }
        public string RequestSecurityConstant {  get; set; }
    }
}
