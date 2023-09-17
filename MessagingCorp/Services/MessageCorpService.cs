using Serilog.Events;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagingCorp.Utils.Logger;
using Ninject;
using MessagingCorp.Configuration;
using MessagingCorp.Configuration.BO;
using MessagingCorp.Modules;

namespace MessagingCorp.Services
{
    public class MessageCorpService
    {
        private IKernel kernel;

        private static readonly ILogger Logger = Log.Logger.ForContextWithConfig<MessageCorpService>("./Logs/MessageCorpService.log", true, LogEventLevel.Debug);

        private IMessageCorpConfiguration messageCorpConfiguration;

        #region Initialization
        public void InitializeService()
        {
            InitializeDI();

            var dbConfig = (DatabaseConfiguration)messageCorpConfiguration.GetConfiguration(MessageCorpConfigType.Database);
            Logger.Information($"DbConfig, DatabaseName: {dbConfig.DatabaseName}");

        }

        private void InitializeDI()
        {
            kernel = new StandardKernel(new MessageCorpServiceModule());
            messageCorpConfiguration = kernel.Get<IMessageCorpConfiguration>();
        }

        #endregion

        #region Service Main

        public void StartOperation()
        {

        }

        #endregion
    }
}
