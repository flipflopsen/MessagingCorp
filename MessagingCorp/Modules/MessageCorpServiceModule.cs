using MessagingCorp.Configuration;
using MessagingCorp.Configuration.BO;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Modules
{
    public class MessageCorpServiceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IMessageCorpConfiguration>().To<MessagingCorpConfig>();
        }
    }
}
