using MessagingCorp.Configuration;
using MessagingCorp.Configuration.BO;
using MessagingCorp.Providers.API;
using MessagingCorp.Providers;
using MessagingCorp.Services.API;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagingCorp.Services.Core;

namespace MessagingCorp.Modules
{
    public class MessageCorpServiceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IMessageCorpConfiguration>().To<MessagingCorpConfig>();
            Bind<IMessageCorpController>().To<MessageCorpController>();
            Bind<IMessageBusProvider>().To<MessageBusProvider>().InSingletonScope();
        }
    }
}
