using Serilog.Events;
using Serilog;
using MessagingCorp.Utils.Logger;
using Ninject;
using MessagingCorp.Configuration;
using MessagingCorp.Configuration.BO;
using MessagingCorp.Modules;
using MessagingCorp.Providers.API;

namespace MessagingCorp.Services
{
    public class MessageCorpService
    {
        private IKernel kernel;

        private static readonly ILogger Logger = Log.Logger.ForContextWithConfig<MessageCorpService>("./Logs/MessageCorpService.log", true, LogEventLevel.Debug);

        private IMessageCorpConfiguration? messageCorpConfiguration;
        private IMessageBusProvider? busProvider;

        #region Initialization
        public void InitializeService()
        {
            InitializeDI();

            var dbConfig = (DatabaseConfiguration)messageCorpConfiguration!.GetConfiguration(MessageCorpConfigType.Database);
            Logger.Information($"DbConfig, DatabaseName: {dbConfig.DatabaseName}");

        }

        private void InitializeDI()
        {
            kernel = new StandardKernel(new MessageCorpServiceModule(), new MessagingBusModule());
            messageCorpConfiguration = kernel.Get<IMessageCorpConfiguration>();
            busProvider = kernel.Get<IMessageBusProvider>();
        }

        #endregion

        #region Service Main

        public void StartOperation()
        {

        }

        #endregion
    }
}
