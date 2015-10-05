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
    public class LogEntryIndexationService: ILogEntryIndexationService
    {
        private static readonly ILogger Logger = Log.Logger.ForContext<LogEntryIndexationService>();
        private readonly IDirectoryProvider _directoryProvider;
        private readonly IDocumentConverter<LogEntry> _logEntryDocumentConverter;

        public LogEntryIndexationService(IDirectoryProvider directoryProvider, IDocumentConverter<LogEntry> logEntryDocumentConverter)
        {
            if (directoryProvider == null) throw new ArgumentNullException(nameof(directoryProvider));
            if (logEntryDocumentConverter == null) throw new ArgumentNullException(nameof(logEntryDocumentConverter));
            _directoryProvider = directoryProvider;
            _logEntryDocumentConverter = logEntryDocumentConverter;
        }

        public Unit IndexOne(LogEntry entry)
        {
            return IndexMany(new[] { entry }.ToImmutableList());
        }

        public Unit IndexMany(IReadOnlyList<LogEntry> entries)
        {
            Logger.Debug("Indexing {count} documents", entries.Count);

            using (var directory = _directoryProvider.Provide())
            using (var analyzer = new StandardAnalyzer(LUCENE_30))
            using (var writer = new IndexWriter(directory, analyzer, UNLIMITED))
            {
                entries.ToList().ForEach(entry => writer.AddDocument(_logEntryDocumentConverter.Convert(entry)));
                return unit;
            }
        }
        
    }

}