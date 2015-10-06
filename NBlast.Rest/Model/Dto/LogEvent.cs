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
        public string Level { get; set; }

        [JsonProperty("Timestamp")]
        public DateTime CreationDate { get; set; } = UtcNow;

        [JsonProperty("Exception")]
        public string Exception { get; set; }

        [JsonProperty("MessageTemplateTokens")]
        public IEnumerable<LogEventTemplateToken> TemplateTokens { get; set; } = Empty<LogEventTemplateToken>().ToImmutableList();

        [JsonProperty("Properties")]
        public IEnumerable<LogEventProperty> Properties { get; set; } = Empty<LogEventProperty>().ToImmutableList();

    }
}