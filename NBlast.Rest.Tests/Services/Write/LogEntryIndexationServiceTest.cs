using System.Linq;
using FluentAssertions;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Util;
using Moq;
using NBlast.Rest.Index;
using NBlast.Rest.Tools;
using NBlast.Rest.Model.Converters;
using NBlast.Rest.Model.Write;
using NBlast.Rest.Services.Write;
using Xunit;
using static Lucene.Net.Documents.Field.Index;
using static Lucene.Net.Documents.Field.Store;
using static Lucene.Net.Util.Version;
using static Moq.MockBehavior;

namespace NBlast.Rest.Tests.Services.Write
{

    public class LogEntryIndexationServiceTest
    {

        [Fact(DisplayName = "It should index few log entries and then find them as expected")]
        public void Check_IndexMany_end_to_end_conversion()
        {
            // Given
            var logEntries        = new[] { new LogEntry("info", "{}"), new LogEntry("debug", "{}"), new LogEntry("error", "{}") };
            var ramdir            = new FakeDirectoryProvider().Provide();
            var documentConverter = new LogEntryDocumentConverter();
            var directoryProvider = new Mock<IDirectoryProvider>(Strict);
            var sut               = MakeSut(directoryProvider.Object, documentConverter);

            directoryProvider.Setup(x => x.Provide()).Returns(ramdir);

            // When
            sut.IndexMany(logEntries);

            // Then
            using (var analyzer = new StandardAnalyzer(LUCENE_30))
            using (var searcher = new IndexSearcher(ramdir, false))
            {
                MakeSearch(analyzer, searcher, $"{nameof(LogEntry.Level)}", "info OR debuG OR Error").TotalHits.Should().Be(3);
                MakeSearch(analyzer, searcher, $"{nameof(LogEntry.Level)}", "Error").TotalHits.Should().Be(1);
                MakeSearch(analyzer, searcher, $"{nameof(LogEntry.Level)}", "Err*").TotalHits.Should().Be(1);
                MakeSearch(analyzer, searcher, $"{nameof(LogEntry.Level)}", "Luck").TotalHits.Should().Be(0);
            }
            ramdir.DisposeIt();
        }

        [Fact(DisplayName = "It should index log entry and then find it as expected")]
        public void Check_IndexOne_end_to_end_conversion()
        {
            // Given
            var logEntry          = new LogEntry("info", "{}");
            var ramdir            = new FakeDirectoryProvider().Provide();
            var documentConverter = new LogEntryDocumentConverter();
            var directoryProvider = new Mock<IDirectoryProvider>(Strict);
            var sut               = MakeSut(directoryProvider.Object, documentConverter);

            directoryProvider.Setup(x => x.Provide()).Returns(ramdir);

            // When
            sut.IndexOne(logEntry);

            // Then
            using (var analyzer = new StandardAnalyzer(LUCENE_30))
            using (var searcher = new IndexSearcher(ramdir, false))
            {
                MakeSearch(analyzer, searcher, $"{nameof(LogEntry.Level)}", "info")
                    .TotalHits
                    .Should().Be(1);
            }
            ramdir.DisposeIt();
        }

        [Fact(DisplayName = "It should index simple document and find it using the same analyzer for indexation and searching")]
        public void Check_IndexOne_same_standard_analyzer()
        {
            // Given
            var logEntry          = new LogEntry("", "");
            var ramdir            = new FakeDirectoryProvider().Provide();
            var documentConverter = new Mock<IDocumentConverter<LogEntry>>(Strict);
            var directoryProvider = new Mock<IDirectoryProvider>(Strict);
            var sut               = MakeSut(directoryProvider.Object, documentConverter.Object);
            var document          = new Document().ToMonad()
                .Do(x => x.Add(new Field("f1", "test 21", YES, ANALYZED_NO_NORMS)))
                .Value;

            documentConverter.Setup(x => x.Convert(logEntry)).Returns(document);
            directoryProvider.Setup(x => x.Provide()).Returns(ramdir);

            // When
            sut.IndexOne(logEntry);

            // Then
            using (var analyzer = new StandardAnalyzer(LUCENE_30))
            using (var searcher = new IndexSearcher(ramdir, false))
            {
                MakeSearch(analyzer, searcher, "f1", "21 OR test")
                    .TotalHits
                    .Should().Be(1);
            }
            ramdir.DisposeIt();
        }

        [Fact(DisplayName = "It should index simple document as expected")]
        public void Check_IndexOne_simple_document()
        {
            // Given
            var logEntry          = new LogEntry("", "");
            var ramdir            = new FakeDirectoryProvider().Provide();
            var documentConverter = new Mock<IDocumentConverter<LogEntry>>(Strict);
            var directoryProvider = new Mock<IDirectoryProvider>(Strict);
            var sut               = MakeSut(directoryProvider.Object, documentConverter.Object);
            var document          = new Document().ToMonad()
                .Do(x => x.Add(new Field("f1", "test", YES, ANALYZED_NO_NORMS)))
                .Value;

            documentConverter.Setup(x => x.Convert(logEntry)).Returns(document);
            directoryProvider.Setup(x => x.Provide()).Returns(ramdir);

            // When
            sut.IndexOne(logEntry);

            // Then
            using (var searcher = new IndexSearcher(ramdir, false))
            {
                searcher.IndexReader.NumDocs().Should().Be(1);
                searcher
                    .Search(new TermQuery(new Term("f1", "test")), searcher.IndexReader.MaxDoc)
                    .TotalHits
                    .Should()
                    .Be(1);
            }
            ramdir.DisposeIt();
        }

        private static TopDocs MakeSearch(StandardAnalyzer analyzer, IndexSearcher searcher, string field, string query)
        {
            return new QueryParser(LUCENE_30, field, analyzer).ToMonad()
                .With(x => x.Parse(query))
                .With(x => searcher.Search(x, searcher.IndexReader.MaxDoc))
                .Value;
        }

        private ILogEntryIndexationService MakeSut(IDirectoryProvider directoryProvider,
                                                   IDocumentConverter<LogEntry> logEntryDocumentConverter = null)
        {
            return new LogEntryIndexationService(directoryProvider,
                                                 logEntryDocumentConverter ?? new Mock<IDocumentConverter<LogEntry>>(Strict).Object);
        }
    }
}