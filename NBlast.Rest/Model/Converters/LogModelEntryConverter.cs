using System.Collections.Immutable;
using System.Linq;
using NBlast.Rest.Model.Dto;
using NBlast.Rest.Model.Write;
using NBlast.Rest.Services.Write;
using Newtonsoft.Json;

namespace NBlast.Rest.Model.Converters
{
    public interface ILogModelEntryConverter : IConverter<LogModel, LogEntry>
    {
    }

    public class LogModelEntryConverter: ILogModelEntryConverter
    {
        public LogEntry Convert(LogModel logModel)
        {
            return new LogEntry(
                level: logModel.Level,
                data: JsonConvert.SerializeObject(logModel),
                creationDate: logModel.CreationDate,
                exception: logModel.Exception,
                properties: logModel.Properties.Select(x => new LogEntryProperty(x.Name, x.Value)).ToImmutableHashSet(),
                templateTokensTexts: logModel.TemplateTokens.Select(x => x.Text).ToImmutableHashSet(),
                templateTokensProperties: logModel.TemplateTokens.Select(x => x.PropertyName).ToImmutableHashSet()
            );
        }
    }
}