using MessagingCorp.Configuration.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Configuration
{
    public interface IMessageCorpConfiguration
    {
        public BaseConfiguration GetConfiguration(MessageCorpConfigType configType);
    }
}
