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
        private IKernel? _kernel;
        private MessageCorpDriver? driver;

        private bool _isInitialized = false;

        #region Initialization
        public void InitializeService()
        {
            InitializeDiKernels(KernelLevel.Driver);

            InitializeServices();
        }

        #region DI-Init

        private void InitializeDiKernels(KernelLevel kernelLevel)
        {

            /*
            var commonServiceModule = new MessageCorpServiceModule();

            switch (kernelLevel)
            {
                case KernelLevel.Auth:
                    {
                        kernels[KernelLevel.Auth] = new StandardKernel(
                            commonServiceModule,
                            new CryptoModule(),
                            new CachingModule(),
                            new DatabaseModule(false)
                        );
                        break;
                    }
                case KernelLevel.Driver:
                    {
                        kernels[KernelLevel.Driver] = new StandardKernel(
                            commonServiceModule,
                            new CryptoModule(),
                            new CachingModule(),
                            new DatabaseModule(false),
                            new AuthenticationModule(),
                            new UserManagementModule()
                        );

                        break;
                    }
            }

            messageCorpConfiguration = kernels[KernelLevel.Driver].Get<IMessageCorpConfiguration>();
            */

            _kernel = new StandardKernel(
                            new MessageCorpServiceModule(),
                            new CryptoModule(),
                            new CachingModule(),
                            new DatabaseModule(false),
                            new AuthenticationModule(),
                            new UserManagementModule());
        }
        #endregion

        #region Service-Init
        private void InitializeServices()
        {
            // Create all the services here with the kernels and create a new "Driver" class, which orchestrates these

            // Driver init
            driver = _kernel.Get<MessageCorpDriver>();
            _isInitialized = true;
        }

        #endregion

        #endregion

        #region Service Main

        public async Task StartOperation()
        {
            if (!_isInitialized)
                throw new InvalidOperationException("Service wasnt properly initialized!");


            await driver!.RunDriver();
        }

        #endregion
    }
}
