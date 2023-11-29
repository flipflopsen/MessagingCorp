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
using System.Text;

namespace MessagingCorp.Services.Core
{
    public class MessageCorpController : IMessageCorpController
    {
        private static readonly ILogger Logger = Log.Logger.ForContextWithConfig<MessageCorpController>("./Logs/MessageCorpDriver.log", true, LogEventLevel.Debug);

        private readonly IMessageBusProvider bus;

        private readonly CorpHttpServer corpHttpServer;
        private readonly CorpPostRequestParser postParser;

        private readonly string challenge;
        private readonly string securityConstant;

        public MessageCorpController(IKernel kernel)
        {
            bus = kernel.Get<IMessageBusProvider>();

            var encConfig = (EncryptionConfiguration)kernel.Get<IMessageCorpConfiguration>().GetConfiguration(MessageCorpConfigType.Encryption);
            challenge = encConfig.RequestSecurityChallenge;
            securityConstant = encConfig.RequestSecurityConstant;

            corpHttpServer = new CorpHttpServer();
            postParser = new CorpPostRequestParser(challenge, securityConstant);

            var httpConfig = (CorpHttpConfiguration)kernel.Get<IMessageCorpConfiguration>().GetConfiguration(MessageCorpConfigType.CorpHttp);
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

            var chillSem = new SemaphoreSlim(0);

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
                        OriginatorUserId = IdGenerator.GenerateNewUserUid(),
                        AdditionalData = cleanData,
                        Action = ActionToEnumConverter.ConvertToAction(reqForm!.Action)
                    };

                    // Because the action handling happens in the Driver, we Observe for an InternalHttpResponse here, which represents the result of the operation.
                    bus!.GetMessageBus().Observe<InternalHttpResponse>().Subscribe(
                        onNext: value =>
                        {
                            if (value.IsSuccess)
                            {
                                response.StatusCode = 200;
                                response.StatusDescription = "OK";
                                response.OutputStream.Write(Encoding.UTF8.GetBytes(value.Userid), 0, Encoding.UTF8.GetBytes(value.Userid).Length);
                            }
                            else
                                response.StatusCode = 404;

                            chillSem.Release();
                        },
                        onError: error => chillSem.Release(),
                        onCompleted: () => chillSem.Release());

                    // then we publish 
                    await bus!.GetMessageBus().Publish(msg);
                }
            }

            // wait async for the sem, released by the observable
            await chillSem.WaitAsync();
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
