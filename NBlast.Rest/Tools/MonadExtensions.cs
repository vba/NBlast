using System;

namespace NBlast.Rest.Tools
{
    public static class MonadExtensions
    { 
        public static Monad<TOut> With<TIn, TOut>(this Monad<TIn> source,
                                                  Func<TIn, TOut> action) => 
                source.HasValue
                    ? Monad.Bind(action(source.Value)) 
                    : Monad.Empty<TOut>();

        public static Monad<T> Do<T>(this Monad<T> source, Action<T> action)
        {
            if (source.HasValue)
            {
                action(source.Value);
            }
            return source;
        }
        public static Monad<T> If<T>(this Monad<T> source, Func<T, bool> condition) => 
                (source.HasValue && condition(source.Value)) 
                    ? source 
                    : Monad.Empty<T>();

        public static Monad<TOut> If<TIn, TOut>(this Monad<TIn> source, 
                                                Func<TIn, bool> condition,
                                                Func<TIn, TOut> @true,
                                                Func<TIn, TOut> @false)
        {
            if (!source.HasValue)
            {
                return Monad.Empty<TOut>();
            }

            return (condition(source.Value))
                ? @true(source.Value).ToMonad()
                : @false(source.Value).ToMonad();
        }


        public static Monad<T> ToMonad<T>(this T me) => Monad.Bind(me);
    }
}