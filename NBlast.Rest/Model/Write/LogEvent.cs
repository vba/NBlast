using System;
using System.Collections.Immutable;
using static System.DateTime;

namespace NBlast.Rest.Model.Write
{
    public class LogEvent
    {
        public Guid Id { get; } = Guid.NewGuid();

        public string Level { get; }

        public DateTime Timestamp { get; } = UtcNow;

        public string Exception { get; }

        public string MessageTemplate { get; }

        public string Message { get; set; }

        public IImmutableList<LogEventProperty> Properties { get; }

        public LogEvent(string level,
                        string exception = null,
                        string messageTemplate = null,
                        string message = null,
                        DateTime? creationDate = null,
                        params LogEventProperty[] items)
        {
            Level           = level;
            Exception       = exception;
            MessageTemplate = messageTemplate;
            Message         = message;
            Timestamp       = creationDate ?? UtcNow;
            Properties      = items.ToImmutableList();
        }

    }
}