using System;

namespace NBlast.Rest.Tools
{
    public static class MonadExtensions
    {
        public static T Value<T> (this Monad<T> me) where T : class => me.Value;

        public static Monad<TOut> With<TIn, TOut>(this Monad<TIn> source,
                                                  Func<TIn, TOut> action) where TIn : class
            where TOut : class => 
                source.Value != default(TIn) 
                    ? Monad.Bind(action(source.Value)) 
                    : Monad.Bind(default(TOut));

        public static Monad<T> Do<T>(this Monad<T> source, Action<T> action) where T : class
        {
            if (source.Value != default(T))
            {
                action(source.Value);
            }
            return source;
        }
        public static Monad<T> If<T>(this Monad<T> source, Func<T, bool> condition)
            where T : class => 
                (source.Value != default(T) && condition(source.Value)) 
                    ? source 
                    : Monad.Bind(default(T));

        public static Monad<TOut> If<TIn, TOut>(this Monad<TIn> source, 
                                                Func<TIn, bool> condition,
                                                Func<TIn, TOut> @true,
                                                Func<TIn, TOut> @false)
            where TIn : class where TOut : class =>
                (source.Value != default(TIn) && condition(source.Value))
                    ? @true(source.Value).Bind()
                    : @false(source.Value).Bind();

        public static Monad<T> Bind<T>(this T me) where T : class => Monad.Bind(me);
    }
}