using MessagingCorp.Common.Logger;
using Serilog;
using Serilog.Events;
using System.Text;

namespace MessagingCorp.Common.HttpStuff
{
    public class CorpPostRequestParser
    {
        private static readonly ILogger Logger = Log.Logger.ForContextWithConfig<CorpPostRequestParser>("./Logs/CorpHttpServer.log", true, LogEventLevel.Debug);

        private readonly string? challenge;
        private readonly string? secConst;

        //todo: pass kernel and config
        public CorpPostRequestParser(string challengeIn, string constantIn)
        {
            challenge = challengeIn;
            secConst = constantIn;
        }

        public CorpPostRequestFormat? Parse(string contents)
        {
            // Decrypt
            string decrypted = contents;

            var requestFormat = new CorpPostRequestFormat();

            string[] lines = decrypted.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var parts = line.Split(new[] { "==" }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 2)
                    throw new ArgumentException("Invalid message string format.");

                var key = parts[0].Trim();
                var value = parts[1].Trim();

                if (!value.EndsWith(';'))
                    return null;

                value = value.Replace(";", "");

                switch (key)
                {
                    case "UserId":
                        requestFormat.UserId = value;
                        break;
                    case "Challenge":
                        if (!value.Equals(challenge))
                        {
                            Logger.Warning("[CorpPostRequestParser] > Invalid challenge detected!");
                            return null;
                        }
                        requestFormat.Challenge = value;
                        break;
                    case "Action":
                        requestFormat.Action = value;
                        break;
                    case "AdditionalData":
                        try
                        {
                            value = Encoding.UTF8.GetString(Convert.FromBase64String(value));
                        }
                        catch (Exception e)
                        {
                            Logger.Warning("[CorpPostRequestParser] > Invalid b64 in additional data detected!");
                            return null;
                        }
                        if (!ValidateAdditionalDataEnding(value))
                        {
                            Logger.Warning("[CorpPostRequestParser] > Failed to security constant of additional data!");
                            return null;
                        }
                        requestFormat.AdditionalData = value;
                        break;
                    default:
                        throw new ArgumentException($"Unknown key: {key}");
                }
            }

            return requestFormat;
        }

        private bool ValidateAdditionalDataEnding(string additionalData)
        {
            var split = additionalData.Split(';');
            return split[split.Length - 1].Equals($"{challenge}:::{secConst}:::{challenge}");
        }
    }
}
