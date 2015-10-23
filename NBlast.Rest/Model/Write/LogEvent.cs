using System;
using System.Collections.Immutable;
using static System.DateTime;

namespace NBlast.Rest.Model.Write
{
    public class LogEvent
    {
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