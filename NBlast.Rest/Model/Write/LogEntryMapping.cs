using Lucene.Net.Analysis.Standard;
using Lucene.Net.Linq.Analysis;
using Lucene.Net.Linq.Fluent;
using Lucene.Net.Linq.Mapping;
using static Lucene.Net.Util.Version;

namespace NBlast.Rest.Model.Write
{
    public class LogEntryMapperProvider : ILogEntryMapperProvider
    {
        private readonly ClassMap<LogEntry> _classMap = new ClassMap<LogEntry>(LUCENE_30);

        public LogEntryMapperProvider()
        {
            _classMap.Key(x => x.Id);
            _classMap.Property(x => x.Content)
                .AnalyzeWith(new StandardAnalyzer(LUCENE_30))
                .AnalyzedNoNorms()
                .NotStored()
                .WithTermVector
                .PositionsAndOffsets();

            _classMap.Property(x => x.CreationDate)
                .Stored()
                .NotAnalyzed();

            _classMap.Property(x => x.Data)
                .Stored()
                .NotAnalyzed();

            _classMap.Property(x => x.Level)
                .AnalyzeWith(new CaseInsensitiveKeywordAnalyzer())
                .AnalyzedNoNorms()
                .Stored();
        }

        public IDocumentMapper<LogEntry> Provide()
        {
            return _classMap.ToDocumentMapper();
        }
    }
}