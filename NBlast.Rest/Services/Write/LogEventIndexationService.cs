using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using LanguageExt;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using NBlast.Rest.Index;
using NBlast.Rest.Model.Write;
using Serilog;
using static LanguageExt.Prelude;
using static Lucene.Net.Index.IndexWriter.MaxFieldLength;
using static Lucene.Net.Util.Version;

namespace NBlast.Rest.Services.Write
{
    public class LogEventIndexationService: ILogEventIndexationService
    {
        private static readonly ILogger Logger = Log.Logger.ForContext<LogEventIndexationService>();
        private readonly IDirectoryProvider _directoryProvider;
        private readonly IDocumentConverter<LogEvent> _logEntryDocumentConverter;

        public LogEventIndexationService(IDirectoryProvider directoryProvider, IDocumentConverter<LogEvent> logEntryDocumentConverter)
        {
            if (directoryProvider == null) throw new ArgumentNullException(nameof(directoryProvider));
            if (logEntryDocumentConverter == null) throw new ArgumentNullException(nameof(logEntryDocumentConverter));
            _directoryProvider = directoryProvider;
            _logEntryDocumentConverter = logEntryDocumentConverter;
        }

        public Unit IndexOne(LogEvent @event)
        {
            return IndexMany(new[] { @event }.ToImmutableList());
        }

        public Unit IndexMany(IReadOnlyList<LogEvent> events)
        {
            Logger.Debug("Indexing {count} documents", events.Count);

            using (var directory = _directoryProvider.Provide())
            using (var analyzer = new StandardAnalyzer(LUCENE_30))
            using (var writer = new IndexWriter(directory, analyzer, UNLIMITED))
            {
                events.ToList().ForEach(entry => writer.AddDocument(_logEntryDocumentConverter.Convert(entry)));
                return unit;
            }
        }
        
    }

}