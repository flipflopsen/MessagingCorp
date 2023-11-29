using Serilog;
using Serilog.Events;
using MessagingCorp.Configuration.BO;
using System.Reflection;
using System.Text.RegularExpressions;
using MessagingCorp.Utils.Logger;
using MessagingCorp.Configuration.Exceptions;

namespace MessagingCorp.Configuration.Helper
{
    public class ConfigurationParser
    {
        private static readonly ILogger Logger = Log.Logger.ForContextWithConfig<ConfigurationParser>("./Logs/ConfigurationParserLog.log", true, LogEventLevel.Debug);

        public BaseConfiguration Parse(string filePath)
        {
            Logger.Information($"Starting to read Log-File: {filePath}");

            var lines = File.ReadLines(filePath);

            var isFullOpMode = false;

            BaseConfiguration? configuration = null;

            var currentProperty = string.Empty;

            foreach (var line in lines)
            {
                if (line.StartsWith("Conf:=:"))
                {
                    // Handle the configuration name
                    string configName = line.Substring("Conf:=:".Length).Trim();
                    Logger.Information($"Configuration Name: {configName}");

                    switch (configName)
                    {
                        case "Database":
                            Log.Information("Got DatabaseConfig!");
                            configuration = new DatabaseConfiguration();
                            configuration.ConfigurationName = configName;
                            break;
                        case "Caching":
                            configuration = new CachingConfiguration();
                            configuration.ConfigurationName = configName;
                            break;
                        case "Encryption":
                            configuration = new EncryptionConfiguration();
                            configuration.ConfigurationName = configName;
                            break;
                        case "LoadBalance":
                            configuration = new LoadBalanceConfiguration();
                            configuration.ConfigurationName = configName;
                            break;
                        case "CorpHttp":
                            configuration = new CorpHttpConfiguration();
                            configuration.ConfigurationName = configName;
                            break;
                        default:
                            Logger.Error($"Failed to determine config type '{configName}' from Header-Line!");
                            throw new ConfigParsingException("Failed to determine config type from Header-Line");
                    }
                }
                else if (line.StartsWith("ConfOpMode::"))
                {
                    // Handle the "Op::{Full}" line
                    string opMode = line.Substring("ConfOpMode::".Length).Trim();
                    switch (opMode)
                    {
                        case "Full": isFullOpMode = true; break;
                        case "Partial": isFullOpMode = false; break;
                        default:
                            throw new ConfigParsingException(
                                "Failed to determine correct ConfOpMode for config, valid id {Full/Partial}!");
                    }
                    Log.Information("DatabaseConfig is ConfOpMode: Full");
                    isFullOpMode = true;
                }
                else if (line.StartsWith("$-+"))
                {
                    if (isFullOpMode)
                        throw new ConfigParsingException("Using default params in ConfOpMode::Full is prohibited!");
                    // Reset currentProperty to null for default values
                    currentProperty = null;
                }
                else if (line.StartsWith("$-"))
                {
                    // Handle property assignments
                    var match = line.Contains("\"") 
                        // This case is for string values
                        ? Regex.Match(line, @"\$-(\w+)\s*=>\s*""([^""]+)"";")

                        // This case is for int and bool
                        : Regex.Match(line, @"\$-(\w+)\s*=>\s*([^""]+);");


                    if (match.Success)
                    {
                        string propertyName = match.Groups[1].Value;
                        string propertyValue = match.Groups[2].Value;

                        PropertyInfo propertyInfo = configuration!.GetType().GetProperty(propertyName)!;
                        if (propertyInfo.CanWrite)
                        {
                            Type propertyType = propertyInfo.PropertyType;

                            if (propertyType == typeof(bool))
                            {
                                if (bool.TryParse(propertyValue, out var parsedBool))
                                    propertyInfo.SetValue(configuration, parsedBool);

                                else
                                {
                                    Log.Error($"Failed to parse boolean value for property: {propertyName}, will set it to FALSE!");
                                    propertyInfo.SetValue(configuration, false);
                                }

                            }
                            else
                            {
                                // For non-boolean properties, use generic conversion
                                try
                                {
                                    object convertedValue = Convert.ChangeType(propertyValue, propertyType);
                                    propertyInfo.SetValue(configuration, convertedValue);
                                }
                                catch (Exception ex)
                                {
                                    // Handle parsing error for non-boolean types
                                    // ToDo: Fallback to default conf
                                    Log.Error($"Failed to parse value for property {propertyName}: {ex.Message}");
                                }
                            }
                        }

                        // Store the current property for default values
                        currentProperty = propertyName;
                    }
                }
                else if (line.StartsWith("$+"))
                {
                    // These are default values, dunno yet on how to figure

                    // Maybe $+Port will default to let's say 8000 if the config is DbConfig for example
                }
            }

            if (isFullOpMode && IsAnyFieldNull(configuration!))
            {
                Log.Error("Failed to parse configuration in FullOpMode, you forgot some field!");
                throw new ConfigParsingException("Failed to parse configuration in FullOpMode, you forgot some field!");
            }

            return configuration!;
        }

        private static bool IsAnyFieldNull(object obj)
        {
            Type type = obj.GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(obj);
                if (value == null)
                {
                    Log.Error($"Field {field.Name} is null!");
                    return true;
                }
            }

            return false;
        }
    }
}
