using Equ;
using System;

namespace NBlast.Rest.Model.Dto
{

    public class LogEventItem : IEquatable<LogEventItem>
    {
        private static readonly MemberwiseEqualityComparer<LogEventItem> Comparer = MemberwiseEqualityComparer<LogEventItem>.ByProperties;

        public Guid Id { get; } = Guid.NewGuid();

        public string Key { get; }
        public object Value { get; }

        public LogEventItem(string key, object value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));

            Key = key;
            Value = value;
        }

        public bool Equals(LogEventItem other)
        {
            return Comparer.Equals(other);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as LogEventItem);
        }

        public override int GetHashCode()
        {
            return Comparer.GetHashCode(this);
        }

        public override string ToString()
        {
            return $"(key = {Key}, value = {Value}, id = {Id})";
        }
    }
}