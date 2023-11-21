using MessagingCorp.Common.HttpStuff;
using MessagingCorp.Configuration;
using MessagingCorp.Configuration.BO;
using MessagingCorp.Providers.API;
using Ninject;
using Serilog.Events;
using Serilog;
using System.Net;
using MessagingCorp.Utils.Logger;
using MessagingCorp.Controller;
using MessagingCorp.Utils;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using MessagingCorp.BO.BusMessages;
using MessagingCorp.Services.API;
using MessagingCorp.EntityManagement.API;
using MessagingCorp.EntityManagement;
using Org.BouncyCastle.Crypto;

namespace MessagingCorp.Services
{
    public class MessageCorpDriver
    {
        private static readonly ILogger Logger = Log.Logger.ForContextWithConfig<MessageCorpDriver>("./Logs/MessageCorpDriver.log", true, LogEventLevel.Debug);

        private IKernel _kernel;

        private MessageCorpController controller;
        private IAuthenticationGovernment? authenticator;
        private IMessageCorpConfiguration? messageCorpConfiguration;
        private IMessageBusProvider? busPovider;
        private IUserManagement? userManagement;

        public MessageCorpDriver(IKernel kernel)
        {
            _kernel = kernel;
            messageCorpConfiguration = _kernel.Get<IMessageCorpConfiguration>();
            busPovider = _kernel.Get<IMessageBusProvider>();
            authenticator = _kernel.Get<IAuthenticationGovernment>();
            userManagement = _kernel.Get<IUserManagement>();

            controller = new MessageCorpController();
        }

        #region Initialization
        public void InitializeDriver()
        {
            var config = (CorpHttpConfiguration)messageCorpConfiguration!.GetConfiguration(MessageCorpConfigType.CorpHttp);
            controller.InitializeCorpHttp(busPovider, config);
        }

        public async Task RunDriver()
        {
            busPovider!.GetMessageBus().Subscribe<RegisterUserMessage>(async (message, tok) => await RegisterUser(message));

            await controller.RunCorpHttp();
        }

        #endregion

        #region Actions

        private async Task RegisterUser(RegisterUserMessage message)
        {
            Logger.Information("Got registerUsermessage for user: " + message.UserName);
            userManagement!.AddUser(message.UserName, message.Password);
            Logger.Information("going to register user..");
            
        }
        private async Task Login()
        {
            /*
            Logger.Information("Got registerUsermessage for user: " + message.UserName);
            if (authenticator!.AuthenticateUser(message.UserName, message.Password))
            {
                userManagement.AddUser(message.UserName, message.Password);
                Logger.Information("going to register user..");
            }
            */
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
