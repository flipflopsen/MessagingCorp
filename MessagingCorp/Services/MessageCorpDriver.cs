using MessagingCorp.Common.HttpStuff;
using MessagingCorp.Configuration;
using MessagingCorp.Configuration.BO;
using MessagingCorp.Providers.API;
using Ninject;

namespace MessagingCorp.Services
{
    public class MessageCorpDriver
    {
        private IKernel _kernel;

        public static MessageCorpDriver Instance { get; private set; }
        private static CorpHttpServer corpHttpServer;
        private IMessageCorpConfiguration? messageCorpConfiguration;
        private IMessageBusProvider? busPovider;

        public MessageCorpDriver(IKernel kernel) 
        {
            _kernel = kernel;
        }

        public MessageCorpDriver()
        {
            Instance = new MessageCorpDriver();
            messageCorpConfiguration = _kernel.Get<IMessageCorpConfiguration>();
            busPovider = _kernel.Get<IMessageBusProvider>();
        }

        public void InitializeDriver()
        {
            var config = (CorpHttpConfiguration)messageCorpConfiguration.GetConfiguration(MessageCorpConfigType.CorpHttp);
            string[] endpoints = new string[] 
            { 
                $"http://{config.CorpHttpIp}:{config.CorpHttpPort}",
                $"http://{config.CorpHttpIp}:{config.CorpHttpPort}/genPoster",
                $"http://{config.CorpHttpIp}:{config.CorpHttpPort}/genGetter"
            };
        }


    }
}
