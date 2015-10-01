using System;

namespace NBlast.Rest.Tools
{
    public sealed class Monad<T> where T: class
    {
        public T Value { get; }
        public Monad(T value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            Value = value;
        }
    }

    public static class Monad
    {
        public static Monad<T> Bind<T>(T value) where T : class
        {
            return new Monad<T>(value);
        }
    }

}