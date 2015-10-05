using FluentAssertions;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Moq;
using NBlast.Rest.Index;
using NBlast.Rest.Tools;
using NBlast.Rest.Model.Converters;
using NBlast.Rest.Model.Write;
using NBlast.Rest.Services.Write;
using Xunit;
using static Lucene.Net.Documents.Field.Index;
using static Lucene.Net.Documents.Field.Store;
using static Moq.MockBehavior;

namespace NBlast.Rest.Tests.Services.Write
{

    public class LogEntryIndexationServiceTest
    {
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
            (ramdir).DisposeIt();
        }

        private ILogEntryIndexationService MakeSut(IDirectoryProvider directoryProvider,
                                                   IDocumentConverter<LogEntry> logEntryDocumentConverter = null)
        {
            return new LogEntryIndexationService(directoryProvider,
                                                 logEntryDocumentConverter ?? new Mock<IDocumentConverter<LogEntry>>(Strict).Object);
        }
    }
}