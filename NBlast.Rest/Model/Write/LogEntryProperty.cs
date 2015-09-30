using System;

namespace NBlast.Rest.Model.Write
{
    public class LogEntryProperty
    {
        public string Name { get; }
        public string Value { get; }
        public LogEntryProperty(string name, string value)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (value == null) throw new ArgumentNullException(nameof(value));
            Name = name;
            Value = value;
        }

        protected bool Equals(LogEntryProperty other)
        {
            return string.Equals(Name, other.Name) && string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((LogEntryProperty) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name?.GetHashCode() ?? 0) * 397) ^ (Value?.GetHashCode() ?? 0);
            }
        }

        public static bool operator ==(LogEntryProperty left, LogEntryProperty right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LogEntryProperty left, LogEntryProperty right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"Name: {Name}, Value: {Value}";
        }
    }
}