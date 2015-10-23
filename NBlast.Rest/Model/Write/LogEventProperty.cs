using System;

namespace NBlast.Rest.Model.Write
{

    public class LogEventProperty
    {
        public Guid Id { get; } = Guid.NewGuid();

        public string Key { get; }
        public object Value { get; }

        public LogEventProperty(string key, object value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            Key = key;
            Value = value;
        }

        protected bool Equals(LogEventProperty other)
        {
            return Tuple.Create(other.Id, other.Key, other.Value)
                .Equals(Tuple.Create(Id, Key, Value));
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((LogEventProperty)obj);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Id, Key, Value).GetHashCode();
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
            return $"(key = {Key}, value = {Value}, id = {Id})";
        }
    }
}