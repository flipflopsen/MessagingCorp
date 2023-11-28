using MessagingCorp.BO.BusMessages;
using MessagingCorp.Common.HttpStuff;
using MessagingCorp.Configuration;
using MessagingCorp.Configuration.BO;
using MessagingCorp.Providers.API;
using MessagingCorp.Services.API;
using MessagingCorp.Utils;
using MessagingCorp.Utils.Converters;
using MessagingCorp.Utils.Logger;
using Ninject;
using Serilog;
using Serilog.Events;
using System.Net;

namespace MessagingCorp.Controller
{
    public class MessageCorpController : IMessageCorpController
    {
        private static readonly ILogger Logger = Log.Logger.ForContextWithConfig<MessageCorpController>("./Logs/MessageCorpDriver.log", true, LogEventLevel.Debug);

        private readonly IKernel _kernel;
        private readonly IMessageBusProvider bus;

        private CorpHttpServer corpHttpServer;
        private CorpPostRequestParser postParser;

        private string challenge = "Challenge";
        private string securityConstant = "SomeMessageCorpConstant";

        public MessageCorpController(IKernel kernel) 
        {
            _kernel = kernel;
            bus = kernel.Get<IMessageBusProvider>();

            var encConfig = (EncryptionConfiguration)_kernel.Get<IMessageCorpConfiguration>().GetConfiguration(MessageCorpConfigType.Encryption);
            challenge = encConfig.RequestSecurityChallenge;
            securityConstant = encConfig.RequestSecurityConstant;

            corpHttpServer = new CorpHttpServer();
            postParser = new CorpPostRequestParser(challenge, securityConstant);

            var httpConfig = (CorpHttpConfiguration)_kernel.Get<IMessageCorpConfiguration>().GetConfiguration(MessageCorpConfigType.CorpHttp);
            corpHttpServer.RegisterEndpoint($"http://{httpConfig.CorpHttpIp}:{httpConfig.CorpHttpPort}/", GenericHandler);
            corpHttpServer.RegisterEndpoint($"http://{httpConfig.CorpHttpIp}:{httpConfig.CorpHttpPort}/genGetter/", GenericGetHandler);
            corpHttpServer.RegisterEndpoint($"http://{httpConfig.CorpHttpIp}:{httpConfig.CorpHttpPort}/genPoster/", GenericPostHandler);

        }

        public async Task RunCorpHttp()
        {
            await corpHttpServer.StartAsync();
        }

        #region HttpHandlers
        private Task<HttpListenerResponse> GenericHandler(HttpListenerContext context)
        {
            Logger.Warning($"[MessageCorpDriver] > Detected unwanted visit in GENERIC from: {context.Request.RemoteEndPoint.Address}");
            return Task.FromResult<HttpListenerResponse>(null!);
        }

        private async Task<HttpListenerResponse> GenericGetHandler(HttpListenerContext context)
        {
            Logger.Warning($"[MessageCorpDriver] > Detected unwanted visit GET from: {context.Request.RemoteEndPoint.Address}");
            return null!;
        }

        private async Task<HttpListenerResponse> GenericPostHandler(HttpListenerContext context)
        {
            Logger.Warning($"[MessageCorpDriver] > Detected unwanted visit in POST from: {context.Request.RemoteEndPoint.Address}");

            var request = context.Request;
            var response = context.Response;

            if (!request.HasEntityBody)
            {
                return null!;
            }

            using (var body = request.InputStream)
            {
                using (var reader = new StreamReader(body, request.ContentEncoding))
                {
                    var content = reader.ReadToEnd();
                    var reqForm = postParser.Parse(content);
                    var cleanData = RemoveVerificationFromAdditionalData(reqForm.AdditionalData);

                    var msg = new CorpMessage()
                    {
                        OriginatorUserId = UserIdGenerator.GenerateNewUserUid(),
                        AdditionalData = cleanData,
                        Action = ActionToEnumConverter.ConvertToAction(reqForm!.Action)
                    };

                    await bus!.GetMessageBus().Publish(msg);

                }
            }
            response.StatusCode = 200;
            response.StatusDescription = "OK";

            return response;
        }

        #endregion

        private string RemoveVerificationFromAdditionalData(string additionalData) 
        {
            // access challenge str
            var challenge = this.challenge;

            // access other security constant
            var securityConstant = this.securityConstant;

            var str = challenge + ":::" + securityConstant + ":::" + challenge;

            return additionalData.Replace(str, "");
        }

    }
}
