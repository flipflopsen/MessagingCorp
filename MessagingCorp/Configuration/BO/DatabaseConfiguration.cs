using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Configuration.BO
{
    public class DatabaseConfiguration : BaseConfiguration
    {
        public string Server { get; set; }
        public string Port { get; set; }
        public string DatabaseName { get; set; }
        public string NameSpace { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RpcTls { get; set; }
        public string LogFilePath { get; set; }
    }
}
