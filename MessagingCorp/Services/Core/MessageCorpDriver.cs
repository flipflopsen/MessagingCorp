using MessagingCorp.BO.BusMessages;
using MessagingCorp.Configuration;
using MessagingCorp.Crypto.Symmetric;
using MessagingCorp.EntityManagement.API;
using MessagingCorp.Providers.API;
using MessagingCorp.Services.API;
using MessagingCorp.Utils.Logger;
using Ninject;
using Serilog;
using Serilog.Events;
using System.Text;

namespace MessagingCorp.Services.Core
{
    public class MessageCorpDriver
    {
        private static readonly ILogger Logger = Log.Logger.ForContextWithConfig<MessageCorpDriver>("./Logs/MessageCorpDriver.log", true, LogEventLevel.Debug);

        private readonly IMessageCorpController controller;
        private readonly IAuthenticationGovernment authenticator;
        private readonly IMessageCorpConfiguration? messageCorpConfiguration;
        private readonly IMessageBusProvider? busPovider;
        private readonly IUserManagement? userManagement;
        private readonly ICryptoProvider? cryptoProvider;
        private readonly IDatabaseAccess? databaseAccess;


        [Inject]
        public MessageCorpDriver(
            IAuthenticationGovernment authenticator,
            IMessageCorpConfiguration messageCorpConfiguration,
            IMessageBusProvider busPovider,
            IUserManagement userManagement,
            IMessageCorpController controller,
            ICryptoProvider cryptoProvider,
            IDatabaseAccess databaseAccess
            )
        {
            this.authenticator = authenticator;
            this.messageCorpConfiguration = messageCorpConfiguration;
            this.busPovider = busPovider;
            this.userManagement = userManagement;
            this.controller = controller;
            this.databaseAccess = databaseAccess;
        }

        #region Initialization
        public void InitializeDriver()
        {
            //controller.InitializeCorpHttp();
        }

        public async Task RunDriver()
        {
            await databaseAccess!.SetupSurrealTables();
            busPovider!.GetMessageBus().Subscribe<CorpMessage>(async (message, tok) => await HandleCorpMessage(message));
            await controller.RunCorpHttp();
        }

        #endregion

        #region Actions

        private async Task HandleCorpMessage(CorpMessage message)
        {
            await Task.Run(() =>
            {
                if (message.Action == Utils.Enumeration.CorpUserAction.Invalid)
                {
                    Logger.Error("[MessageCorpDriver] > Action field from CorpMessage was invalid in handler!");
                    throw new NullReferenceException("Action field from CorpMessage was null in handler!");
                }

                switch (message.Action)
                {
                    case Utils.Enumeration.CorpUserAction.RegisterUser:
                        {
                            var usernamePasswordSplit = message.AdditionalData.Split(";");
                            Logger.Information("Got registerUsermessage for user: " + message);
                            userManagement!.AddUser(message.OriginatorUserId!, usernamePasswordSplit[0], usernamePasswordSplit[1]);
                            Logger.Information("going to register user..");

                            var usr = userManagement!.GetUser(message.OriginatorUserId!);

                            busPovider!.GetMessageBus().Publish(new InternalHttpResponse() { IsSuccess = true, Userid = message.OriginatorUserId! });
                            break;
                        }
                    case Utils.Enumeration.CorpUserAction.LoginUser:
                        {
                            Logger.Information("Got registerUsermessage for user: " + message.OriginatorUserId);
                            if (authenticator!.AuthenticateUser(message.OriginatorUserId, message.Password))
                            {

                            }
                            break;
                        }
                    case Utils.Enumeration.CorpUserAction.SendMessage:
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
            });
        }

        #endregion
    }
}
