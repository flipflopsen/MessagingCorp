using MessagingCorp.Configuration;
using MessagingCorp.Configuration.BO;
using MessagingCorp.Modules;
using MessagingCorp.Utils.Enumeration;
using MessagingCorp.Utils.Logger;
using Ninject;
using Serilog;
using Serilog.Events;
using System.Collections.Concurrent;

namespace MessagingCorp.Services.Core
{
    public class MessageCorpService
    {
        private static readonly ILogger Logger = Log.Logger.ForContextWithConfig<MessageCorpService>("./Logs/MessageCorpService.log", true, LogEventLevel.Debug);

        private readonly ConcurrentDictionary<KernelLevel, IKernel> kernels = new ConcurrentDictionary<KernelLevel, IKernel>();
        private MessageCorpDriver? driver;

        private IMessageCorpConfiguration? messageCorpConfiguration;

        private bool _isInitialized = false;

        #region Initialization
        public async Task InitializeService()
        {
            InitializeDiKernels(KernelLevel.Driver);

            var dbConfig = (DatabaseConfiguration)messageCorpConfiguration!.GetConfiguration(MessageCorpConfigType.Database);
            Logger.Information($"DbConfig, DatabaseName: {dbConfig.DatabaseName}");

            await InitializeServices();
        }

        #region DI-Init

        private void InitializeDiKernels(KernelLevel kernelLevel)
        {
            var commonServiceModule = new MessageCorpServiceModule();

            switch (kernelLevel)
            {
                case KernelLevel.Auth:
                    {
                        kernels[KernelLevel.Auth] = new StandardKernel(
                            commonServiceModule,
                            new CryptoModule(),
                            new CachingModule(),
                            new DatabaseModule()
                        );
                        break;
                    }
                case KernelLevel.Driver:
                    {
                        kernels[KernelLevel.Driver] = new StandardKernel(
                            commonServiceModule,
                            new CryptoModule(),
                            new CachingModule(),
                            new DatabaseModule(),
                            new AuthenticationModule(),
                            new UserManagementModule()
                        );

                        break;
                    }
            }

            messageCorpConfiguration = kernels[KernelLevel.Driver].Get<IMessageCorpConfiguration>();
        }
        #endregion

        #region Service-Init
        private async Task InitializeServices()
        {
            // Create all the services here with the kernels and create a new "Driver" class, which orchestrates these

            // Driver init
            driver = kernels[KernelLevel.Driver].Get<MessageCorpDriver>();
            driver.InitializeDriver();
            await driver.RunDriver();


            _isInitialized = true;
        }

        #endregion

        #endregion

        #region Service Main

        public async Task StartOperation()
        {
            if (!_isInitialized)
                throw new InvalidOperationException("Service wasnt properly initialized!");

            driver!.InitializeDriver();

            await driver.RunDriver();
        }

        #endregion
    }
}
