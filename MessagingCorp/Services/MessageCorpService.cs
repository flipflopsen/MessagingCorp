using Serilog.Events;
using Serilog;
using MessagingCorp.Utils.Logger;
using Ninject;
using MessagingCorp.Configuration;
using MessagingCorp.Configuration.BO;
using MessagingCorp.Modules;
using MessagingCorp.Providers.API;
using System.Collections.Concurrent;

namespace MessagingCorp.Services
{
    public class MessageCorpService
    {
        private readonly ConcurrentDictionary<KernelLevel, IKernel> kernels = new ConcurrentDictionary<KernelLevel, IKernel>();
        private MessageCorpDriver driver;

        private static readonly ILogger Logger = Log.Logger.ForContextWithConfig<MessageCorpService>("./Logs/MessageCorpService.log", true, LogEventLevel.Debug);

        private IMessageCorpConfiguration? messageCorpConfiguration;
        private IMessageBusProvider? busProvider;

        #region Initialization
        public void InitializeService()
        {
            InitializeDiKernels();

            var dbConfig = (DatabaseConfiguration)messageCorpConfiguration!.GetConfiguration(MessageCorpConfigType.Database);
            Logger.Information($"DbConfig, DatabaseName: {dbConfig.DatabaseName}");

            InitializeServices();
        }

        private void InitializeDiKernels()
        {
            var commonConfigModule = new MessageCorpServiceModule();
            var commonMessageBusModule = new MessagingBusModule();

            // Happens sync so direct dict access is ok here
            kernels[KernelLevel.Driver] = new StandardKernel(
                commonConfigModule,
                commonMessageBusModule
                );

            kernels[KernelLevel.Auth] = new StandardKernel(
                commonConfigModule,
                new CryptoModule(),
                new CachingModule(),
                new DatabaseModule()
                );

            kernels[KernelLevel.All] = new StandardKernel(
                commonConfigModule,
                commonMessageBusModule,
                new CryptoModule(),
                new CachingModule(),
                new DatabaseModule(),
                new AuthenticationModule(),
                new UserManagementModule()
                );

            messageCorpConfiguration = kernels[KernelLevel.Driver].Get<IMessageCorpConfiguration>();
            busProvider = kernels[KernelLevel.Driver].Get<IMessageBusProvider>();

        }

        private void InitializeServices()
        {
            // Create all the services here with the kernels and create a new "Driver" class, which orchestrates these

            driver = new MessageCorpDriver(kernels[KernelLevel.All]);
        }

        #endregion

        #region Service Main

        public async Task StartOperation()
        {
            driver.InitializeDriver();

            await driver.RunDriver();
        }

        #endregion
    }
}
