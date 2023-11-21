using MessagingCorp.BO.BusMessages;
using MessagingCorp.Common.HttpStuff;
using MessagingCorp.Configuration.BO;
using MessagingCorp.Providers.API;
using MessagingCorp.Utils.Logger;
using Serilog;
using Serilog.Events;
using System.Net;

namespace MessagingCorp.Controller
{
    public class MessageCorpController
    {
        private static readonly ILogger Logger = Log.Logger.ForContextWithConfig<MessageCorpController>("./Logs/MessageCorpDriver.log", true, LogEventLevel.Debug);

        private CorpHttpServer corpHttpServer;
        private CorpPostRequestParser postParser;
        private IMessageBusProvider bus;

        public MessageCorpController() 
        { 
            corpHttpServer = new CorpHttpServer();
            postParser = new CorpPostRequestParser("123");
        }
        public void InitializeCorpHttp(IMessageBusProvider bus, CorpHttpConfiguration config)
        {
            this.bus = bus;
            corpHttpServer.RegisterEndpoint($"http://{config.CorpHttpIp}:{config.CorpHttpPort}/", GenericHandler);
            corpHttpServer.RegisterEndpoint($"http://{config.CorpHttpIp}:{config.CorpHttpPort}/genGetter/", GenericGetHandler);
            corpHttpServer.RegisterEndpoint($"http://{config.CorpHttpIp}:{config.CorpHttpPort}/genPoster/", GenericPostHandler);
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

                    switch (reqForm!.Action)
                    {
                        case "RegisterUser": await bus!.GetMessageBus().Publish(new RegisterUserMessage(reqForm.UserId, reqForm.AdditionalData)); break;
                    }
                }
            }
            response.StatusCode = 200;
            response.StatusDescription = "OK";

            return response;
        }

        #endregion



    }
}
