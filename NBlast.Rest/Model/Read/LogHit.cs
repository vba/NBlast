using System;
using Newtonsoft.Json;

namespace NBlast.Rest.Model.Read
{
    public class LogHit
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonIgnore]
        public string Data { get; set; }
        [JsonProperty("level")]
        public string Level { get; set; }
        [JsonProperty("boost")]
        public float Boost { get; set; }
        [JsonProperty("score")]
        public float Score { get; set; }
        [JsonProperty("creationDate")]
        public DateTime CreationDate { get; set; }
    }
}