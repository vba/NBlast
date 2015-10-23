using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using LanguageExt;
using NBlast.Rest.Model.Write;
using Serilog;
using static LanguageExt.Prelude;

namespace NBlast.Rest.Async
{
    public class IndexingQueueKeeper: IIndexingQueueKeeper
    {
        private static readonly ILogger Logger = Log.Logger.ForContext<IndexingQueueKeeper>();
        private readonly ConcurrentQueue<LogEvent> _queue = new ConcurrentQueue<LogEvent>();

        public Option<LogEvent> Consume()
        {
            LogEvent result;
            return _queue.TryDequeue(out result) ? Some(result) : None;
        }

        public Option<LogEvent> Peek()
        {
            LogEvent result;
            return _queue.TryPeek(out result) ? Some(result) : None;
        }

        public int Count() => _queue.Count;

        public IReadOnlyList<LogEvent> ConsumeTop(int top = 10) =>
            Range(0, top - 1)
                .Select(x => Consume())
                .Select(x => x.Match(Some: y => y,
                                     None: () => null))
                .Where(x => x != null)
                .ToImmutableList();
        

        public IReadOnlyList<LogEvent> PeekTop(int top = 10) => _queue.Take(top).ToImmutableList();

        public Unit Enqueue(LogEvent entry)
        {
            Logger.Debug("Enqueuing model @{0}", entry);
            _queue.Enqueue(entry);
            return unit;
        }
    }
}