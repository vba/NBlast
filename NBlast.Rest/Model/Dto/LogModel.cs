using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace NBlast.Rest.Model.Dto
{
    public class LogModel
    {
        [Required]
        [JsonProperty("level")]
        public string Level { get; set; }
        [JsonProperty("creationDate")]
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        [Required]
        [JsonProperty("data")]
        public dynamic Data { get; set; }

        public override string ToString()
        {
            return $"Level: {Level}, CreationDate: {CreationDate}, Data: {Data}";
        }
    }
}