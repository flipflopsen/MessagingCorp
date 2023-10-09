using Serilog.Events;
using Serilog;
using System.Net;
using System.Text;
using MessagingCorp.Utils.Logger;

namespace MessagingCorp.Common.HttpStuff
{
    public class CorpHttpServer
    {
        private static readonly ILogger Logger = Log.Logger.ForContextWithConfig<CorpHttpServer>("./Logs/CorpHttpServer.log", true, LogEventLevel.Debug);

        private readonly Dictionary<string, Func<HttpListenerRequest, Task<HttpListenerResponse>>> registeredEndpoints;
        private readonly HttpListener listener;
        private bool isRunning;

        public CorpHttpServer(string[] endpoints)
        {
            listener = new HttpListener();
            registeredEndpoints = new Dictionary<string, Func<HttpListenerRequest, Task<HttpListenerResponse>>>();

            // Port is defined in prefixes
            foreach (string endpoint in endpoints)
            {
                listener.Prefixes.Add(endpoint);
            }
        }

        public void RegisterEndpoint(string path, Func<HttpListenerRequest, Task<HttpListenerResponse>> handler)
        {
            registeredEndpoints[path.ToLower()] = handler;
        }

        public async Task StartAsync()
        {
            listener.Start();
            Logger.Debug("CorpHttpServer is listening");

            isRunning = true;

            while (isRunning)
            {
                var context = await listener.GetContextAsync();
                await ProcessRequestAsync(context);
            }
        }

        private async Task ProcessRequestAsync(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;
            var path = request.Url.AbsolutePath.ToLower();
            var clientIp = context.Request.RemoteEndPoint.Address.ToString();

            if (path == null)
            {
                Logger.Warning($"(NullPath) Caught a weird request on endpoint: - from IP: {clientIp}");
                response.StatusCode = (int)HttpStatusCode.Forbidden;
                response.StatusDescription = "Forbidden";
                var responseText = "Forbidden, Endpoint and IP logged.";
                var responseData = Encoding.UTF8.GetBytes(responseText);
                response.ContentLength64 = responseData.Length;
                await response.OutputStream.WriteAsync(responseData, 0, responseData.Length);
                return;
            }

            if (registeredEndpoints.ContainsKey(path!))
            {
                // Call the registered handler for the requested endpoint
                var handler = registeredEndpoints[path];
                var responseTask = handler(request);

                // Await and send the response when the handler completes
                var httpResponse = await responseTask;
                await SendHttpResponseAsync(httpResponse, response);
            }
            else
            {
                Logger.Warning($"Caught a weird request on endpoint: {path} from IP: {clientIp}");
                response.StatusCode = (int)HttpStatusCode.Forbidden;
                response.StatusDescription = "Forbidden";
                var responseText = "Forbidden, Endpoint and IP logged.";
                var responseData = Encoding.UTF8.GetBytes(responseText);
                response.ContentLength64 = responseData.Length;
                await response.OutputStream.WriteAsync(responseData, 0, responseData.Length);
            }

            response.Close();
        }

        private async Task SendHttpResponseAsync(HttpListenerResponse httpResponse, HttpListenerResponse response)
        {
            response.StatusCode = httpResponse.StatusCode;
            response.StatusDescription = httpResponse.StatusDescription;
            response.ContentType = httpResponse.ContentType;

            using (var outputStream = response.OutputStream)
            using (var inputStream = httpResponse.OutputStream)
            {
                await inputStream.CopyToAsync(outputStream);
            }
        }


        public void Stop()
        {
            isRunning = false;
            listener.Stop();
            listener.Close();
        }
    }
}
