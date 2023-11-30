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
using System.Runtime;

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
            await Task.Delay(1);
            return null!;
        }

        private async Task<HttpListenerResponse> GenericPostHandler(HttpListenerContext context)
        {
            IDisposable observerDisposable;
            Logger.Information($"[MessageCorpController] > Received POST-Request from: {context.Request.RemoteEndPoint.Address}");

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
                    var cleanData = RemoveVerificationFromAdditionalData(reqForm!.AdditionalData);

                    
                    var msg = new CorpMessage()
                    {
                        OriginatorUserId = reqForm.UserId == "0" ? IdGenerator.GenerateNewUserUid() : reqForm.UserId,
                        AdditionalData = cleanData,
                        Action = ActionToEnumConverter.ConvertToAction(reqForm!.Action)
                    };

                    // TODO: can this cause shit when a lot of requests occur at the same time?
                    // Because the action handling happens in the Driver, we Observe for an InternalHttpResponse here, which represents the result of the operation.
                    observerDisposable = bus!.GetMessageBus().Observe<InternalHttpResponse>().Subscribe(
                        onNext: value =>
                        {
                            if (value.IsSuccess)
                            {
                                response.StatusCode = 200;
                                response.StatusDescription = "OK";
                                response.OutputStream.Write(Encoding.UTF8.GetBytes(value.ResponseString), 0, Encoding.UTF8.GetBytes(value.ResponseString).Length);
                            }
                            else
                                response.StatusCode = 404;

                            //Logger.Debug("[MessageCorpController] > sem gets released cuz onNext");
                            chillSem.Release();
                        },
                        onError: error =>
                            {
                                //Logger.Debug("[MessageCorpController] > sem gets released cuz onError");
                                chillSem.Release();
                            },
                        onCompleted: () =>
                            {
                                //Logger.Debug("[MessageCorpController] > sem gets released cuz onCompleted");
                                chillSem.Release();
                            });

                    // then we publish 
                    await bus!.GetMessageBus().Publish(msg);
                }
            }

            // TODO: can this cause shit when a lot of requests occur at the same time?
            //Logger.Debug("[MessageCorpController] > Waiting for chillsem");
            await chillSem.WaitAsync();
            chillSem = new SemaphoreSlim(0);
            observerDisposable!.Dispose();
            return response;
        }

        #endregion

        #region Helpers
        private string RemoveVerificationFromAdditionalData(string additionalData)
        {
            // access challenge str
            var challenge = this.challenge;

            // access other security constant
            var securityConstant = this.securityConstant;

            var str = challenge + ":::" + securityConstant + ":::" + challenge;

            return additionalData.Replace(str, "");
        }

        #endregion
    }
}
