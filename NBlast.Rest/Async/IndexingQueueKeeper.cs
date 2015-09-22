using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using LanguageExt;
using NBlast.Rest.Model.Dto;
using Serilog;
using static LanguageExt.Prelude;

namespace NBlast.Rest.Async
{
    public class IndexingQueueKeeper: IIndexingQueueKeeper
    {
        private static readonly ILogger Logger = Log.Logger.ForContext<IndexingQueueKeeper>();
        private readonly ConcurrentQueue<LogModel> _queue = new ConcurrentQueue<LogModel>();

        public Option<LogModel> Consume()
        {
            LogModel result;
            return _queue.TryDequeue(out result) ? Some(result) : None;
        }

        public Option<LogModel> Peek()
        {
            LogModel result;
            return _queue.TryPeek(out result) ? Some(result) : None;
        }

        public int Count() => _queue.Count;

        public IReadOnlyList<LogModel> ConsumeTop(int top = 10) =>
            range(0, top - 1)
                .Select(x => Consume())
                .Select(x => x.Match(Some: y => y,
                                     None: () => null))
                .Where(x => x != null)
                .ToImmutableList();
        

        public IReadOnlyList<LogModel> PeekTop(int top = 10) => _queue.Take(top).ToImmutableList();

        public Unit Enqueue(LogModel entry)
        {
            Logger.Debug("Enqueuing model @{0}", entry);
            _queue.Enqueue(entry);
            return unit;
        }
    }
}