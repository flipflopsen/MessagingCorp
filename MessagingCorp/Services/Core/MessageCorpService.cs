using MessagingCorp.Common.Logger;
using MessagingCorp.Modules;
using Ninject;
using Serilog;
using Serilog.Events;

namespace MessagingCorp.Services.Core
{
    public class MessageCorpService
    {
        private static readonly ILogger Logger = Log.Logger.ForContextWithConfig<MessageCorpService>("./Logs/MessageCorpService.log", true, LogEventLevel.Debug);

        private IKernel? _kernel;
        private MessageCorpDriver? driver;

        private bool _isInitialized = false;

        #region Initialization
        public void InitializeService()
        {
            InitializeDiKernel();

            InitializeServices();
        }

        #region DI-Init

        private void InitializeDiKernel()
        {
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
