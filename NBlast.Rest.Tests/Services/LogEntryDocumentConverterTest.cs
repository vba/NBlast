using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Lucene.Net.Documents;
using NBlast.Rest.Model.Write;
using NBlast.Rest.Services.Write;
using Ploeh.AutoFixture;
using static NBlast.Rest.Services.ServiceConstant;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace NBlast.Rest.Tests.Services
{
    public class LogEntryDocumentConverterTest
    {

        [Theory(DisplayName = "It should convert log entry's template properties to document multivalue field as expected"), AutoData]
        public void Convert_check_template_properties(string level,
                                                      string data,
                                                      string exception,
                                                      DateTime creationDate)
        {
            // Given
            var values = new Fixture().CreateMany<string>(3).ToArray();
            var sut = MakeSut();
            var logEntry = new LogEntry(level,
                                        data,
                                        creationDate,
                                        exception,
                                        templateTokensProperties: new HashSet<string>(values));

            // When
            var document = sut.Convert(logEntry);

            // Then
            document.GetFields().Count.Should().Be(10);
            document.GetValues(FieldNames.TemplateTokensProperty).Should().HaveCount(values.Count());
        }
        [Theory(DisplayName = "It should convert log entry's template texts to document multivalue field as expected"), AutoData]
        public void Convert_check_template_texts(string level,
                                                 string data,
                                                 string exception,
                                                 DateTime creationDate)
        {
            // Given
            var values = new Fixture().CreateMany<string>(3).ToArray();
            var sut = MakeSut();
            var logEntry = new LogEntry(level,
                                        data,
                                        creationDate,
                                        exception,
                                        templateTokensTexts: new HashSet<string>(values));

            // When
            var document = sut.Convert(logEntry);

            // Then
            document.GetFields().Count.Should().Be(10);
            document.GetValues(FieldNames.TemplateTokensText).Should().HaveCount(values.Count());
        }


        [Theory(DisplayName = "It should convert log entry's properties to document fields as expected"), AutoData]
        public void Convert_check_properties(string level,
                                             string data,
                                             string exception,
                                             DateTime creationDate)
        {
            // Given
            var sut = MakeSut();
            var logEntry = new LogEntry(level,
                                        data,
                                        creationDate,
                                        exception,
                                        new HashSet<LogEntryProperty>
                                        {
                                            new LogEntryProperty("duplicate", "value"),
                                            new LogEntryProperty("duplicate", "value"),
                                            new LogEntryProperty("key1", 300000000000000000.5m),
                                            new LogEntryProperty("key2", creationDate),
                                            new LogEntryProperty("key3", "some"),
                                        });

            // When
            var document = sut.Convert(logEntry);

            // Then
            document.GetFields().Count.Should().Be(11);
            document.Get($"{FieldNames.Propertiy}.duplicate").ShouldBeEquivalentTo("value");
            document.Get($"{FieldNames.Propertiy}.key1").ShouldBeEquivalentTo("300000000000000000.5");
            document.Get($"{FieldNames.Propertiy}.key2").ShouldBeEquivalentTo(DateTools.DateToString(creationDate, DateTools.Resolution.SECOND));
            document.Get($"{FieldNames.Propertiy}.key3").ShouldBeEquivalentTo("some");
        }

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
            document.GetFields().Count.Should().Be(7);
            document.Get(nameof(LogEntry.Level)).ShouldBeEquivalentTo(level);
            document.Get(nameof(LogEntry.Data)).ShouldBeEquivalentTo(data);
            document.Get(nameof(LogEntry.Exception)).ShouldBeEquivalentTo(exception);
            document.Get(nameof(LogEntry.CreationDate)).ShouldBeEquivalentTo(DateTools.DateToString(creationDate, DateTools.Resolution.SECOND));
            document.Get(FieldNames.Type).ShouldBeEquivalentTo(typeof(LogEntry).Name);
        }

        [Theory(DisplayName = "It should convert simple log entry to lucene document without exception"),
         InlineData("debug", "{}", "", null),
         InlineData("debug", "{}", null, null)]
        public void Convert_check_without_exception(string level,
                                                    string data,
                                                    string exception,
                                                    DateTime? creationDate)
        {
            // Given
            var sut = MakeSut();
            var logEntry = new LogEntry(level, data, creationDate, exception);

            // When
            var document = sut.Convert(logEntry);

            // Then
            document.GetFields().Count.Should().Be(6);
            document.Get(nameof(LogEntry.Level)).ShouldBeEquivalentTo(level);
            document.Get(nameof(LogEntry.Data)).ShouldBeEquivalentTo(data);
            document.Get(nameof(LogEntry.Exception)).Should().BeNull();
            document.Get(nameof(LogEntry.CreationDate)).Should().NotBeNull();
        }

        private LogEntryDocumentConverter MakeSut()
        {
            return new LogEntryDocumentConverter();
        }
    }
}