using MessagingCorp.Common.Logger;
using Serilog;
using Serilog.Events;
using System.Net;
using System.Text;

namespace MessagingCorp.Common.HttpStuff
{
    public class CorpHttpServer : IDisposable
    {
        private static readonly ILogger Logger = Log.Logger.ForContextWithConfig<CorpHttpServer>("./Logs/CorpHttpServer.log", true, LogEventLevel.Debug);

        private readonly Dictionary<string, Func<HttpListenerContext, Task<HttpListenerResponse>>> registeredEndpoints;
        private readonly HttpListener listener;
        private bool isRunning;
        private bool disposedValue;

        public CorpHttpServer()
        {
            listener = new HttpListener();
            registeredEndpoints = new Dictionary<string, Func<HttpListenerContext, Task<HttpListenerResponse>>>();
        }

        public CorpHttpServer(string[] endpoints)
        {
            listener = new HttpListener();
            registeredEndpoints = new Dictionary<string, Func<HttpListenerContext, Task<HttpListenerResponse>>>();

            // Port is defined in prefixes
            foreach (string endpoint in endpoints)
            {
                listener.Prefixes.Add(endpoint);
            }
        }

        public void RegisterEndpoint(string path, Func<HttpListenerContext, Task<HttpListenerResponse>> handler)
        {
            var split = path.Split("/");
            var endpoint = split[split.Length - 2];

            if (endpoint.Contains(":"))
                endpoint = "";

            endpoint = "/" + endpoint.ToLower();
            registeredEndpoints[endpoint] = handler;

            listener.Prefixes.Add(path);
        }

        public async Task StartAsync()
        {
            listener.Start();
            Logger.Debug("[CorpHttpServer] > CorpHttpServer is listening");

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
            var path = request!.Url!.AbsolutePath.ToLower();
            var clientIp = context.Request.RemoteEndPoint.Address.ToString();

            if (path == null)
            {
                Logger.Warning($"[CorpHttpServer] > (NullPath) Caught a weird request on endpoint: - from IP: {clientIp}");

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
                var responseTask = handler(context);

                // Await and send the response when the handler completes
                _ = await responseTask;
            }
            else
            {
                Logger.Warning($"[CorpHttpServer] > Caught a weird request on endpoint: {path} from IP: {clientIp}");
                response.StatusCode = (int)HttpStatusCode.Forbidden;
                response.StatusDescription = "Forbidden";

                var responseText = "Forbidden, Endpoint and IP logged.";
                var responseData = Encoding.UTF8.GetBytes(responseText);

                response.ContentLength64 = responseData.Length;

                await response.OutputStream.WriteAsync(responseData, 0, responseData.Length);
            }

            response.Close();
        }

        public void Stop()
        {
            isRunning = false;
            listener.Stop();
            listener.Close();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Stop();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
