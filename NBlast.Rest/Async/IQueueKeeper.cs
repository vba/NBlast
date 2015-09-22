using System.Collections.Generic;
using LanguageExt;

namespace NBlast.Rest.Async
{
    public interface IQueueKeeper<T>
    {
        Unit Enqueue(T entry);
        Option<T> Consume();
        Option<T> Peek();
        int Count();
        IReadOnlyList<T> ConsumeTop(int top = 10);
        IReadOnlyList<T> PeekTop(int top = 10);
    }
}