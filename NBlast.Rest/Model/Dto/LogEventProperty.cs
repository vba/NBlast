using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace NBlast.Rest.Model.Dto
{
    public class LogEventProperty
    {
        [Required]
        [JsonProperty("Name")]
        public string Name { get; set; }

        [Required]
        [JsonProperty("Value")]
        public object Value { get; set; }

        protected bool Equals(LogEventProperty other)
        {
            return string.Equals(Name, other.Name) && Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((LogEventProperty) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name?.GetHashCode() ?? 0) * 397) ^ (Value?.GetHashCode() ?? 0);
            }
        }

        public static bool operator ==(LogEventProperty left, LogEventProperty right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LogEventProperty left, LogEventProperty right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"Name: {Name}, Value: {Value}";
        }
    }
}