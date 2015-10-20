using System.Collections.Immutable;
using System.Linq;
using NBlast.Rest.Model.Dto;
using NBlast.Rest.Model.Write;
using NBlast.Rest.Services.Write;
using Newtonsoft.Json;

namespace NBlast.Rest.Model.Converters
{
    public interface ILogModelEntryConverter : IConverter<LogEvent, LogEntry>
    {
    }

    public class LogModelEntryConverter: ILogModelEntryConverter
    {
        public LogEntry Convert(LogEvent logEvent)
        {
            return new LogEntry(
                level                   : logEvent.Level,
                data                    : JsonConvert.SerializeObject(logEvent),
                creationDate            : logEvent.Timestamp,
                exception               : logEvent.Exception,
                properties              : logEvent._OldProperties.Select(x => new LogEntryProperty(x.Name, x.Value)).ToImmutableHashSet(),
                templateTokensTexts     : logEvent._OldTemplateTokens.Select(x => x.Text).ToImmutableHashSet(),
                templateTokensProperties: logEvent._OldTemplateTokens.Select(x => x.PropertyName).ToImmutableHashSet()
            );
        }
    }
}