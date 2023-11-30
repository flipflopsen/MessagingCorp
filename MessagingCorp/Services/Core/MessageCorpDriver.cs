using MessagingCorp.BO.BusMessages;
using MessagingCorp.Configuration;
using MessagingCorp.Crypto.Symmetric;
using MessagingCorp.Database.API;
using MessagingCorp.EntityManagement.API;
using MessagingCorp.Providers.API;
using MessagingCorp.Services.API;
using MessagingCorp.Common.Logger;
using Ninject;
using Serilog;
using Serilog.Events;
using System.Text;
using MessagingCorp.Common.Enumeration;

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
            // noop atm
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
            await Task.Run(async () =>
            {
                if (message.Action == CorpUserAction.Invalid)
                {
                    Logger.Error("[MessageCorpDriver] > Action field from CorpMessage was invalid in handler!");
                }

                switch (message.Action)
                {
                    case CorpUserAction.RegisterUser:
                        {
                            Logger.Information("[MessageCorpDriver] > Got registerUser-Message for user: " + message);

                            var usernamePasswordSplit = message.AdditionalData.Split(";");

                            await userManagement!.AddUser(message.OriginatorUserId!, usernamePasswordSplit[0], usernamePasswordSplit[1]);

                            await busPovider!.GetMessageBus().Publish(new InternalHttpResponse() { IsSuccess = true, Userid = message.OriginatorUserId!, ResponseString = message.OriginatorUserId!});
                            break;
                        }
                    case CorpUserAction.LoginUser:
                        {
                            Logger.Information($"[MessageCorpDriver] > Got login-Message for user: {message.OriginatorUserId}");

                            var usernamePasswordSplit = message.AdditionalData.Split(";");
                            message.Password = usernamePasswordSplit[1];
                            message.OriginatorUserName = usernamePasswordSplit[0];

                            if (await authenticator!.AuthenticateUser(message.OriginatorUserId, message.Password))
                            {
                                Logger.Information($"[MessageCorpDriver] > User {message.OriginatorUserId} ({message.OriginatorUserName}) authenticated successfully!");
                                await busPovider!.GetMessageBus().Publish(new InternalHttpResponse() { IsSuccess = true, Userid = message.OriginatorUserId!, ResponseString = "login successful"});
                            }
                            else
                            {
                                Logger.Warning($"[MessageCorpDriver] > Failed to authenticate user with uid: {message.OriginatorUserId}!");
                                await busPovider!.GetMessageBus().Publish(new InternalHttpResponse() { IsSuccess = false, Userid = message.OriginatorUserId!, ResponseString = "login failed" });
                            }
                            break;
                        }
                    case CorpUserAction.AddFriendRequest:
                        {
                            Logger.Information("[MessageCorpDriver] > Got addFriend-Message for user: " + message);
                            message = PopulateMessageByAction(message);

                            if (!await authenticator!.IsUserAuthenticated(message.OriginatorUserId!))
                            {
                                await busPovider!.GetMessageBus().Publish(new InternalHttpResponse() { IsSuccess = false, Userid = message.OriginatorUserId!, ResponseString = "not authenticated" });
                                break;
                            }

                            await userManagement!.SendFriendRequest(message.OriginatorUserId!, message.TargetUserId);

                            await busPovider!.GetMessageBus().Publish(new InternalHttpResponse() { IsSuccess = true, Userid = message.OriginatorUserId!, ResponseString = $"sent friend request to {message.TargetUserId}" });

                            break;
                        }
                    case CorpUserAction.AcceptFriendRequest:
                        {
                            Logger.Information("[MessageCorpDriver] > Got acceptFriend-Message for user: " + message);

                            message = PopulateMessageByAction(message);
                            if (!await authenticator!.IsUserAuthenticated(message.OriginatorUserId!))
                            {
                                await busPovider!.GetMessageBus().Publish(new InternalHttpResponse() { IsSuccess = false, Userid = message.OriginatorUserId!, ResponseString = "not authenticated" });
                                break;
                            }

                            await userManagement!.AcceptFriendRequest(message.OriginatorUserId!, message.TargetUserId);
                            await busPovider!.GetMessageBus().Publish(new InternalHttpResponse() { IsSuccess = true, Userid = message.OriginatorUserId!, ResponseString = $"accepted friend request from {message.TargetUserId}" });

                            break;
                        }
                    case CorpUserAction.SendMessage:
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

        #region Helper

        public CorpMessage PopulateMessageByAction(CorpMessage corpMessage)
        {
            switch (corpMessage.Action)
            {
                case CorpUserAction.AddFriendRequest:
                case CorpUserAction.AcceptFriendRequest:
                    {
                        corpMessage.TargetUserId = corpMessage.AdditionalData.Replace(";", "");
                        break;
                    }
                default:
                    break;
            }

            return corpMessage;
        }
        #endregion
    }
}
