using MessagingCorp.Common.HttpStuff;
using MessagingCorp.Configuration;
using MessagingCorp.Configuration.BO;
using MessagingCorp.Providers.API;
using Ninject;
using Serilog.Events;
using Serilog;
using System.Net;
using MessagingCorp.Utils.Logger;

namespace MessagingCorp.Services
{
    public class MessageCorpDriver
    {
        private static readonly ILogger Logger = Log.Logger.ForContextWithConfig<MessageCorpDriver>("./Logs/MessageCorpDrivedr.log", true, LogEventLevel.Debug);

        private IKernel _kernel;

        public static MessageCorpDriver Instance { get; private set; }

        private static CorpHttpServer corpHttpServer;
        private IMessageCorpConfiguration? messageCorpConfiguration;
        private IMessageBusProvider? busPovider;

        public MessageCorpDriver(IKernel kernel)
        {
            _kernel = kernel;
            Instance = new MessageCorpDriver(kernel);
            messageCorpConfiguration = _kernel.Get<IMessageCorpConfiguration>();
            busPovider = _kernel.Get<IMessageBusProvider>();
        }

        #region Initialization
        public void InitializeDriver()
        {
            InitializeCorpHttp();
        }

        private void InitializeCorpHttp()
        {
            var config = (CorpHttpConfiguration)messageCorpConfiguration.GetConfiguration(MessageCorpConfigType.CorpHttp);
            string[] endpoints = new string[]
            {
                $"http://{config.CorpHttpIp}:{config.CorpHttpPort}",
                $"http://{config.CorpHttpIp}:{config.CorpHttpPort}/genPoster",
                $"http://{config.CorpHttpIp}:{config.CorpHttpPort}/genGetter"
            };

            corpHttpServer.RegisterEndpoint($"http://{config.CorpHttpIp}:{config.CorpHttpPort}", GenericHandler);
            corpHttpServer.RegisterEndpoint($"http://{config.CorpHttpIp}:{config.CorpHttpPort}/genGetter", GenericGetHandler);
            corpHttpServer.RegisterEndpoint($"http://{config.CorpHttpIp}:{config.CorpHttpPort}/genPoster", GenericPostHandler);
            
        }
        #endregion

        #region HttpHandlers
        private Task<HttpListenerResponse> GenericHandler(HttpListenerRequest request)
        {
            Logger.Warning($"[MessageCorpDriver] > Detected unwanted visit from: {request.RemoteEndPoint.Address}");
            return Task.FromResult<HttpListenerResponse>(null!);
        }

        private async Task<HttpListenerResponse> GenericGetHandler(HttpListenerRequest request)
        {

            return null;
        }

        private async Task<HttpListenerResponse> GenericPostHandler(HttpListenerRequest request)
        {

            return null;
        }

        #endregion

        #region Actions

        private void RegisterUser()
        {

        }
        private void Login()
        {

        }

        private void SendMessage()
        {

        }

        private void CreateLobby()
        {

        }

        private void JoinLobby()
        {

        }

        private void PurgeUser()
        {

        }

        private void UpdateChatMessages()
        {

        }

        private void ChangeSymEncryptionMethod()
        {

        }

        private void ChangeAsymEncryptionMethod()
        {

        }

        #endregion
    }
}
