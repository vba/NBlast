using System;

namespace NBlast.Rest.Tools
{
    public sealed class Monad<T>
    {
        public static readonly Monad<T> Empty = new Monad<T>();
        public bool HasValue { get; }
        public T Value { get; }
        public Monad(T value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            Value = value;
            HasValue = true;
        }

        private Monad()
        {
            HasValue = false;
        }

        public T ValueOrElse(T @else) => HasValue ? Value : @else;
    }

    public static class Monad
    {
        public static Monad<T> Result<T>(T value) => 
            value != null ? new Monad<T>(value) : Empty<T>();

        public static Monad<T> Empty<T> () => Monad<T>.Empty;
    }

}