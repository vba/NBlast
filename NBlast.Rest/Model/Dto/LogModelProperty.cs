using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace NBlast.Rest.Model.Dto
{
    public class LogModelProperty
    {
        [Required]
        [JsonProperty("Name")]
        public string Name { get; set; }

        [Required]
        [JsonProperty("Value")]
        public object Value { get; set; }

        protected bool Equals(LogModelProperty other)
        {
            return string.Equals(Name, other.Name) && Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((LogModelProperty) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name?.GetHashCode() ?? 0) * 397) ^ (Value?.GetHashCode() ?? 0);
            }
        }

        public static bool operator ==(LogModelProperty left, LogModelProperty right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LogModelProperty left, LogModelProperty right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"Name: {Name}, Value: {Value}";
        }
    }
}