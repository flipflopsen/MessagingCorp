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

        public string DefaultCorpAesKeySize { get; set; }
        public string DefaultCorpAesKey { get; set; }
        public string DefaultCorpAesIv {  get; set; }
        public string DefaultCorpRsaKeySize { get; set; }
        public string DefaultCorpRsaPubKey { get; set; }
        public string DefaultCorpRsaPrivKey { get; set; }
    }
}
