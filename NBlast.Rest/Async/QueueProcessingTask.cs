using System;
using System.Collections.Generic;
using System.Diagnostics;
using FluentScheduler;
using LanguageExt;
using NBlast.Rest.Model.Write;
using NBlast.Rest.Services.Write;
using Serilog;
using static LanguageExt.Prelude;

namespace NBlast.Rest.Async
{
    public interface IQueueProcessingTask : ITask
    {
    }

    public class QueueProcessingTask: IQueueProcessingTask
    {
        private readonly IIndexingQueueKeeper _indexingQueueKeeper;
        private readonly ILogEventIndexationService _logEntryIndexationService;
        private static readonly ILogger Logger = Log.Logger.ForContext<QueueProcessingTask>();

        public QueueProcessingTask(IIndexingQueueKeeper indexingQueueKeeper,
                                   ILogEventIndexationService logEntryIndexationService)
        {
            if (indexingQueueKeeper == null) throw new ArgumentNullException(nameof(indexingQueueKeeper));
            if (logEntryIndexationService == null) throw new ArgumentNullException(nameof(logEntryIndexationService));

            _indexingQueueKeeper = indexingQueueKeeper;
            _logEntryIndexationService = logEntryIndexationService;
        }

        private Unit ProcessModels(IReadOnlyList<LogEvent> events)
        {
            var sw = new Stopwatch();
            sw.Start();

            _logEntryIndexationService.IndexMany(events);

            sw.Stop();
            Logger.Debug($"Import process has took ${sw.ElapsedMilliseconds} msec(s)");
            return unit;
        }

        public void Execute()
        {
            var count = _indexingQueueKeeper.Count();
            if(count < 1) return;
            Logger.Verbose("Scheduled task executed, queue contains {0} element(s)", count);
            ProcessModels(_indexingQueueKeeper.ConsumeTop(400));
        }
    }
}