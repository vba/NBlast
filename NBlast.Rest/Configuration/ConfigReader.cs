using System;
using System.Configuration;
using Serilog;
using static LanguageExt.Prelude;

namespace NBlast.Rest.Configuration
{
    public class ConfigReader : IConfigReader
    {
        private static readonly ILogger Logger = Log.Logger.ForContext<ConfigReader>();

        public string Read(string key)
        {
            try
            {
                var appSetting = ConfigurationManager.AppSettings[key];
                return Environment.ExpandEnvironmentVariables(appSetting);
            }
            catch (Exception e)
            {
                Logger.Error(e, $"App setting not found for key {key}");
                throw;
            }
        }

        public int ReadAsInt(string key)
        {
            return parseInt(Read(key)).Match(
                Some: x => x,
                None: () => {
                    var message = $"Cannot conver setting {key} to {typeof(int)}";
                    var exception = new InvalidOperationException(message);
                    Logger.Error(exception, message);
                    throw exception;
                } 
            );
        }
    }
}