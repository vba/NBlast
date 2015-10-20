using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using static System.DateTime;
using static System.Linq.Enumerable;

namespace NBlast.Rest.Model.Dto
{
    public class LogEvent
    {
        [Required]
        [JsonProperty("level")]
        public string Level { get; }

        [JsonProperty("Timestamp")]
        public DateTime Timestamp { get; } = UtcNow;

        [JsonProperty("Exception")]
        public string Exception { get; }

        [Obsolete]
        [JsonProperty("MessageTemplate")]
        public IEnumerable<LogEventTemplateToken> _OldTemplateTokens { get; set; } = Empty<LogEventTemplateToken>().ToImmutableList();

        [Obsolete]
        [JsonProperty("Properties")]
        public IEnumerable<LogEventProperty> _OldProperties { get; set; } = Empty<LogEventProperty>().ToImmutableList();

        public string MessageTemplate { get; }

        public IImmutableList<LogEventItem> Properties { get; }

        public LogEvent(string level,
                        string exception,
                        string messageTemplate,
                        DateTime? creationDate,
                        params LogEventItem[] items)
        {
            Level           = level;
            Exception       = exception;
            MessageTemplate = messageTemplate;
            Timestamp    = creationDate ?? UtcNow;
            Properties      = items.ToImmutableList();
        }

    }
}