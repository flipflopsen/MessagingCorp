using MessagingCorp.Providers;
using MessagingCorp.Providers.API;
using Ninject.Modules;

namespace MessagingCorp.Modules
{
    public class MessagingBusModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IMessageBusProvider>().To<MessageBusProvider>().InSingletonScope();
        }
    }
}
