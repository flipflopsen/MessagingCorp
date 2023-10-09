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
        private const string HTTP_CONF = "CorpHttp";
        private const string CONFIG_EXTENSION = ".corpconf";

        private DatabaseConfiguration? DatabaseConfiguration { get; set; }
        private EncryptionConfiguration? EncryptionConfiguration { get; set; }
        private LoadBalanceConfiguration? LoadBalanceConfiguration { get; set; }
        private CachingConfiguration? CachingConfiguration { get; set; }
        private CorpHttpConfiguration? CorpHttpConfiguration { get; set; }

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

            switch (configType)
            {
                case MessageCorpConfigType.Encryption: 
                    if (EncryptionConfiguration == null)
                        EncryptionConfiguration = (EncryptionConfiguration) ParseConfig(Path.Combine(configFolderPath, ENC_CONF + CONFIG_EXTENSION)) ?? new EncryptionConfiguration();
                    return EncryptionConfiguration;
                case MessageCorpConfigType.LoadBalance: 
                    if (LoadBalanceConfiguration == null)
                        LoadBalanceConfiguration = (LoadBalanceConfiguration) ParseConfig(Path.Combine(configFolderPath, LB_CONF) + CONFIG_EXTENSION) ?? new LoadBalanceConfiguration();
                    return LoadBalanceConfiguration;
                case MessageCorpConfigType.Database: 
                    if (DatabaseConfiguration == null)
                        DatabaseConfiguration = (DatabaseConfiguration) ParseConfig(Path.Combine(configFolderPath, DB_CONF + CONFIG_EXTENSION)) ?? new DatabaseConfiguration();
                    return DatabaseConfiguration;
                case MessageCorpConfigType.Caching: 
                    if (CachingConfiguration  == null)
                        CachingConfiguration = (CachingConfiguration) ParseConfig(Path.Combine(configFolderPath, CH_CONF + CONFIG_EXTENSION)) ?? new CachingConfiguration(); 
                    return CachingConfiguration;
                case MessageCorpConfigType.CorpHttp: 
                    if (CorpHttpConfiguration == null)
                        CorpHttpConfiguration = (CorpHttpConfiguration) ParseConfig(Path.Combine(configFolderPath, HTTP_CONF + CONFIG_EXTENSION)) ?? new CorpHttpConfiguration();
                    return CorpHttpConfiguration;
                case MessageCorpConfigType.General: 
                    throw new ConfigChoiceException();
                default: 
                    throw new ConfigChoiceException();

            }
        }
    }
}
