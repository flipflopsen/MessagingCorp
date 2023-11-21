using Serilog.Events;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagingCorp.Utils.Logger;

namespace MessagingCorp.Common.HttpStuff
{
    public class CorpPostRequestParser
    {
        private static readonly ILogger Logger = Log.Logger.ForContextWithConfig<CorpPostRequestParser>("./Logs/CorpHttpServer.log", true, LogEventLevel.Debug);

        private readonly string? challenge;

        public CorpPostRequestParser(string challengeIn)
        {
            challenge = challengeIn;
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
                        if (!ValidateAdditionalDataEnding(value))
                        {
                            Logger.Warning("[CorpPostRequestParser] > Failed to validate ending of additional data!");
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
            return true;
        }
    }
}
