using MessagingCorp.Common.EventBus;
using MessagingCorp.Providers;
using MessagingCorp.Providers.API;
using MessagingCorp.Services.API;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Modules
{
    public class MessagingBusModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IMessageBusProvider>().To<MessageBusProvider>();
        }
    }
}
