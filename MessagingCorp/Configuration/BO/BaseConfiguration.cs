using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Configuration.BO
{
    public abstract class BaseConfiguration
    {
        public string ConfigurationName { get; set; }
        protected BaseConfiguration() { }
    }
}
