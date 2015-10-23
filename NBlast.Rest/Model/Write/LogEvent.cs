using System;
using System.Collections.Immutable;
using System.Linq;
using static System.DateTime;

namespace NBlast.Rest.Model.Write
{
    public static class LogEventExtensions
    {
        public static string GetContent(this LogEvent me) =>
            me.Properties
                .Aggregate(GetPrimaryFields(me), 
                           (aggregated, x) => $"{aggregated} {x.Value}");

        private static string GetPrimaryFields(LogEvent me) =>
            $"{me.Level} {me.Exception} {me.MessageTemplate}";
        
    }
    public class LogEvent
    {
        public Guid Id { get; } = Guid.NewGuid();

        public string Level { get; }

        public DateTime Timestamp { get; } = UtcNow;

        public string Exception { get; }

        public string MessageTemplate { get; }

        public IImmutableList<LogEventProperty> Properties { get; }

        public LogEvent(string level,
                        string exception,
                        string messageTemplate = null,
                        DateTime? creationDate = null,
                        params LogEventProperty[] items)
        {
            Level           = level;
            Exception       = exception;
            MessageTemplate = messageTemplate;
            Timestamp       = creationDate ?? UtcNow;
            Properties      = items.ToImmutableList();
        }

    }
}