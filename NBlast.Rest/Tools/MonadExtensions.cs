using System;

namespace NBlast.Rest.Tools
{
    public static class MonadExtensions
    {
        public static Monad<TOut> With<TIn, TOut>(this Monad<TIn> source,
                                                  Func<TIn, TOut> func) =>
            source.HasValue
                ? Monad.Result(func(source.Value))
                : Monad.Empty<TOut>();

        public static Monad<TOut> SelectMany<TIn, TOut>(this Monad<TIn> source,
                                                        Func<TIn, Monad<TOut>> func) =>
            source.HasValue
                ? func(source.Value)
                : Monad.Empty<TOut>();

        public static Monad<TOut> SelectMany<TIn, TInter, TOut>(this Monad<TIn> source,
                                                                Func<TIn, Monad<TInter>> intermediary,
                                                                Func<TIn, TInter, TOut> final) => 
            source.SelectMany(x => intermediary(x).SelectMany(y => final(x, y).ToMonad()));
        public static Monad<TOut> SelectMany<TIn, TInter, TOut>(this TIn source,
                                                                Func<TIn, TInter> intermediary,
                                                                Func<TIn, TInter, TOut> final) => 
            !source.ToMonad().HasValue 
                ? Monad.Empty<TOut>()
                : source
                    .ToMonad()
                    .SelectMany(x => intermediary(x).ToMonad().SelectMany(y => final(x, y).ToMonad()));

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
                                                Func<TIn, TOut> follow,
                                                Func<TIn, TOut> otherwise)
        {
            if (!source.HasValue)
            {
                return Monad.Empty<TOut>();
            }

            return (condition(source.Value))
                ? follow(source.Value).ToMonad()
                : otherwise(source.Value).ToMonad();
        }


        public static Monad<T> ToMonad<T>(this T me) => Monad.Result(me);
    }
}