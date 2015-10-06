using System.Collections.Generic;
using Newtonsoft.Json;
using static System.Linq.Enumerable;

namespace NBlast.Rest.Model.Dto
{
    public class LogEvents
    {
        [JsonProperty("events")]
        public IEnumerable<LogEvent> Events { get; set; } = Empty<LogEvent>(); 
    }
}