using MessagingCorp.Configuration.Exceptions;
using MessagingCorp.Configuration.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MessagingCorp.Configuration.BO
{
    public class MessagingCorpConfig : IMessageCorpConfiguration
    {
        private readonly ConfigurationParser _configParser;
        private const string CONFIG_FOLDER_BASE_NAME = "Configuration";
        private const string DB_CONF = "Database";
        private const string ENC_CONF = "Encryption";
        private const string LB_CONF = "LoadBalance";
        private const string CH_CONF = "Caching";
        private const string CONFIG_EXTENSION = ".corpconf";

        private DatabaseConfiguration? DatabaseConfiguration { get; set; }
        private EncryptionConfiguration? EncryptionConfiguration { get; set; }
        private LoadBalanceConfiguration? LoadBalanceConfiguration { get; set; }
        private CachingConfiguration? CachingConfiguration { get; set; }

        public MessagingCorpConfig()
        {
            _configParser = new ConfigurationParser();
        }

        private BaseConfiguration ParseConfig(string filePath)
        {
            if (File.Exists(filePath))
                return _configParser.Parse(filePath);

            throw new ConfigParsingException($"Failed to Parse configuration: {filePath}, shutting down..");
        }

        public BaseConfiguration GetConfiguration(MessageCorpConfigType configType)
        {
            var configFolderPath = Path.Combine(Directory.GetCurrentDirectory(), CONFIG_FOLDER_BASE_NAME);

            return configType switch
            {
                MessageCorpConfigType.Encryption => ParseConfig(Path.Combine(configFolderPath, ENC_CONF + CONFIG_EXTENSION)) ?? new EncryptionConfiguration(),
                MessageCorpConfigType.LoadBalance => ParseConfig(Path.Combine(configFolderPath, LB_CONF) + CONFIG_EXTENSION) ?? new LoadBalanceConfiguration(),
                MessageCorpConfigType.Database => ParseConfig(Path.Combine(configFolderPath, DB_CONF + CONFIG_EXTENSION)) ?? new DatabaseConfiguration(),
                MessageCorpConfigType.Caching => ParseConfig(Path.Combine(configFolderPath, CH_CONF + CONFIG_EXTENSION)) ?? new CachingConfiguration(),
                MessageCorpConfigType.General => throw new ConfigChoiceException(),
                _ => throw new ConfigChoiceException(),
            };
        }
    }
}
