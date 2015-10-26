using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Lucene.Net.Documents;
using NBlast.Rest.Model.Write;
using NBlast.Rest.Services;
using NBlast.Rest.Services.Write;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace NBlast.Rest.Tests.Services.Write
{
    public class LogEventDocumentConverterTest
    {


        [Theory(DisplayName = "It should convert log entry's template property to document field as expected"), AutoData]
        public void Convert_check_template_propertie(string level,
                                                     string messageTemplate,
                                                     string exception,
                                                     DateTime creationDate)
        {
            // Given
            var sut = MakeSut();
            var logEntry = new LogEvent(level,
                                        exception,
                                        messageTemplate,
                                        creationDate);

            // When
            var document = sut.Convert(logEntry);

            // Then
            document.GetFields().Count.Should().Be(6);
            document.GetValues(nameof(LogEvent.MessageTemplate)).Should().HaveCount(1).And.BeEquivalentTo(messageTemplate);
        }

        [Theory(DisplayName = "It should convert log entry's properties to document fields as expected"), AutoData]
        public void Convert_check_properties(string level,
                                             string messageTemplate,
                                             string exception,
                                             DateTime creationDate)
        {
            // Given
            var sut = MakeSut();
            var logEntry = new LogEvent(level,
                                        exception,
                                        messageTemplate,
                                        creationDate,
                                        new LogEventProperty("duplicate", "value"),
                                        new LogEventProperty("duplicate", "value"),
                                        new LogEventProperty("key1", 300000000000000000.5m),
                                        new LogEventProperty("key2", creationDate),
                                        new LogEventProperty("key3", "some"));

            // When
            var document = sut.Convert(logEntry);

            // Then
            document.GetFields().Count.Should().Be(11);
            document.Get($"{nameof(LogEvent.Properties)}.duplicate").ShouldBeEquivalentTo("value");
            document.Get($"{nameof(LogEvent.Properties)}.key1").ShouldBeEquivalentTo("300000000000000000.5");
            document.Get($"{nameof(LogEvent.Properties)}.key2").ShouldBeEquivalentTo(DateTools.DateToString(creationDate, DateTools.Resolution.SECOND));
            document.Get($"{nameof(LogEvent.Properties)}.key3").ShouldBeEquivalentTo("some");
        }

        [Theory(DisplayName = "It should convert simple log entry to lucene document as expected without any data lost"), AutoData]
        public void Convert_check_basic_operation(string level, string exception, DateTime creationDate)
        {
            // Given
            var sut = MakeSut();
            var logEntry = new LogEvent(level, exception, creationDate: creationDate);

            // When
            var document = sut.Convert(logEntry);

            // Then
            document.GetFields().Count.Should().Be(5);
            document.Get(nameof(LogEvent.Level)).ShouldBeEquivalentTo(level);
            document.Get(nameof(LogEvent.Exception)).ShouldBeEquivalentTo(exception);
            document.Get(nameof(LogEvent.Timestamp)).ShouldBeEquivalentTo(DateTools.DateToString(creationDate, DateTools.Resolution.SECOND));
        }

        [Theory(DisplayName = "It should convert simple log entry to lucene document without exception"),
         InlineData("debug", "{}", "", null),
         InlineData("debug", "{}", null, null)]
        public void Convert_check_without_exception(string level,
                                                    string messageTemplate,
                                                    string exception,
                                                    DateTime? creationDate)
        {
            // Given
            var sut = MakeSut();
            var logEntry = new LogEvent(level, exception, messageTemplate, creationDate);

            // When
            var document = sut.Convert(logEntry);

            // Then
            document.GetFields().Count.Should().Be(5);
            document.Get(nameof(LogEvent.Level)).ShouldBeEquivalentTo(level);
            document.Get(nameof(LogEvent.Exception)).Should().BeNull();
            document.Get(nameof(LogEvent.Timestamp)).Should().NotBeNull();
        }

        private LogEventDocumentConverter MakeSut()
        {
            return new LogEventDocumentConverter();
        }
    }
}