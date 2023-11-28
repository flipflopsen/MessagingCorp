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
using MessagingCorp.BO.BusMessages;
using MessagingCorp.Services.API;
using MessagingCorp.EntityManagement.API;
using MessagingCorp.EntityManagement;
using Org.BouncyCastle.Crypto;
using MessagingCorp.Utils.Converters;
using System.Text;
using MessagingCorp.Crypto.Symmetric;

namespace MessagingCorp.Services
{
    public class MessageCorpDriver
    {
        private static readonly ILogger Logger = Log.Logger.ForContextWithConfig<MessageCorpDriver>("./Logs/MessageCorpDriver.log", true, LogEventLevel.Debug);

        private readonly IMessageCorpController controller;
        private readonly IAuthenticationGovernment authenticator;
        private readonly IMessageCorpConfiguration? messageCorpConfiguration;
        private readonly IMessageBusProvider? busPovider;
        private readonly IUserManagement? userManagement;


        [Inject]
        public MessageCorpDriver(
            IAuthenticationGovernment authenticator,
            IMessageCorpConfiguration messageCorpConfiguration,
            IMessageBusProvider busPovider,
            IUserManagement userManagement,
            IMessageCorpController controller
            )
        {
            this.authenticator = authenticator;
            this.messageCorpConfiguration = messageCorpConfiguration;
            this.busPovider = busPovider;
            this.userManagement = userManagement;
            this.controller = controller;
        }

        #region Initialization
        public void InitializeDriver()
        {
            //controller.InitializeCorpHttp();
        }

        public async Task RunDriver()
        {
            busPovider!.GetMessageBus().Subscribe<CorpMessage>(async (message, tok) => await HandleCorpMessage(message));

            await controller.RunCorpHttp();
        }

        #endregion

        #region Actions

        private async Task HandleCorpMessage(CorpMessage message)
        {
            if (message.Action == Utils.Action.Invalid)
            {
                Logger.Error("[MessageCorpDriver] > Action field from CorpMessage was invalid in handler!");
                throw new NullReferenceException("Action field from CorpMessage was null in handler!");
            }

            switch (message.Action)
            {
                case Utils.Action.RegisterUser:
                    {
                        var usernamePasswordSplit = message.AdditionalData.Split(";");
                        Logger.Information("Got registerUsermessage for user: " + message);
                        userManagement!.AddUser(message.OriginatorUserId!, usernamePasswordSplit[0], usernamePasswordSplit[1]);
                        Logger.Information("going to register user..");
                        break;
                    }
                case Utils.Action.LoginUser:
                    {
                        Logger.Information("Got registerUsermessage for user: " + message.OriginatorUserId);
                        if (authenticator!.AuthenticateUser(message.OriginatorUserId, message.Password))
                        {

                        }
                        break;
                    }
                case Utils.Action.SendMessage:
                    {
                        byte[] testKey = Encoding.UTF8.GetBytes("01234567890123456789012345678901");
                        byte[] testIV = Encoding.UTF8.GetBytes("0123456789012345");
                        Logger.Information("Got sendMessage for user: " + message.OriginatorUserId);
                        var aes = new AES(EEncryptionStrategySymmetric.AES_GCM);
                        var originalMessage = Convert.ToBase64String(Encoding.UTF8.GetBytes("Hello, this is a test message!"));

                        // Act
                        var encryptedMessage = aes.EncryptMessage(originalMessage, testKey, testIV);
                        var decryptedMessage = aes.DecryptMessage(encryptedMessage, testKey, testIV);

                        
                        break;

                    }
            }
        }

        #endregion
    }
}
