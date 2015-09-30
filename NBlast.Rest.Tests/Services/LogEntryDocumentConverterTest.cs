using System;
using FluentAssertions;
using Lucene.Net.Documents;
using NBlast.Rest.Model.Write;
using NBlast.Rest.Services.Write;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace NBlast.Rest.Tests.Services
{
    public class LogEntryDocumentConverterTest
    {

        [Theory(DisplayName = "It should convert simple log entry to lucene document as expected without any data lost"), AutoData]
        public void Convert_check_basic_operation(string level,
                                                  string data,
                                                  string exception,
                                                  DateTime creationDate)
        {
            // Given
            var sut = MakeSut();
            var logEntry = new LogEntry(level, data, creationDate, exception);

            // When
            var document = sut.Convert(logEntry);

            // Then
            document.Get(nameof(LogEntry.Level)).ShouldBeEquivalentTo(level);
            document.Get(nameof(LogEntry.Data)).ShouldBeEquivalentTo(data);
            document.Get(nameof(LogEntry.Exception)).ShouldBeEquivalentTo(exception);
            document.Get(nameof(LogEntry.CreationDate)).ShouldBeEquivalentTo(DateTools.DateToString(creationDate, DateTools.Resolution.SECOND));
        }

        private LogEntryDocumentConverter MakeSut()
        {
            return new LogEntryDocumentConverter();
        }
    }
}