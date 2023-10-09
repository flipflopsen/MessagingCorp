using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Configuration.BO
{
    public class CorpHttpConfiguration : BaseConfiguration
    {
        public string CorpHttpIp { get; set; }
        public string CorpHttpPort { get; set; }
        public string LogFilePath { get; set; }
    }
}
