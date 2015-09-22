using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;

namespace NBlast.Rest.Model.Dto
{
    public class LogModel
    {
        [Required]
        [JsonProperty("level")]
        public string Level { get; set; }
        [JsonProperty("Timestamp")]
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        [JsonProperty("MessageTemplateTokens")]
        public IEnumerable<LogModelTemplateToken> TemplateTokens { get; set; } = Enumerable.Empty<LogModelTemplateToken>().ToImmutableList();

        public IEnumerable<LogModelProperty> Properties { get; set; } = Enumerable.Empty<LogModelProperty>().ToImmutableList();

    }
}