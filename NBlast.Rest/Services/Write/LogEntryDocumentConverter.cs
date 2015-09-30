using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Lucene.Net.Documents;
using NBlast.Rest.Model.Write;
using static System.String;
using static Lucene.Net.Documents.Field.Index;
using static Lucene.Net.Documents.Field.Store;
using static Lucene.Net.Documents.Field;
using static NBlast.Rest.Services.ServiceConstant.FieldNames;

namespace NBlast.Rest.Services.Write
{
    public interface IDocumentConverter<in T>
    {
        Document Convert(T entry);
    }

    public class LogEntryDocumentConverter : IDocumentConverter<LogEntry>
    {
        public Document Convert(LogEntry entry)
        {
            var document = StartBuildDocument(entry);

            PrepareProperties(entry)
                .Union(PrepareTemplateTexts(entry))
                .Union(PrepareTemplateProperties(entry))
                .ToImmutableList()
                .ForEach(x => document.Add(x));

            return document;
        }

        private static Document StartBuildDocument(LogEntry entry)
        {
            var document = new Document();

            document.Add(new Field(nameof(LogEntry.Id), entry.Id.ToString(), YES, NOT_ANALYZED_NO_NORMS));
            document.Add(new Field(nameof(LogEntry.Level), entry.Level, YES, ANALYZED_NO_NORMS));
            document.Add(new Field(nameof(LogEntry.Content), entry.Content, Store.NO, ANALYZED_NO_NORMS));
            document.Add(new Field(nameof(LogEntry.Data), entry.Data, YES, NOT_ANALYZED_NO_NORMS));
            document.Add(new Field(Type, typeof (LogEntry).Name, YES, NOT_ANALYZED_NO_NORMS));
            document.Add(new Field(nameof(LogEntry.CreationDate), DateTools.DateToString(entry.CreationDate, DateTools.Resolution.SECOND), Store.NO, ANALYZED_NO_NORMS));

            if (!IsNullOrEmpty(entry.Exception))
            {
                document.Add(new Field(nameof(LogEntry.Exception), entry.Exception, YES, ANALYZED_NO_NORMS));
            }
            return document;
        }

        private static IImmutableList<Field> PrepareTemplateProperties(LogEntry entry)
        {
            return entry.TemplateTokensProperties?
                .Select(x => new Field($"{TemplateTokensProperty}", x, Store.NO, ANALYZED_NO_NORMS))
                .ToImmutableList();
        }
        private static IImmutableList<Field> PrepareTemplateTexts(LogEntry entry)
        {
            return entry.TemplateTokensTexts?
                .Select(x => new Field($"{TemplateTokensText}", x, Store.NO, ANALYZED_NO_NORMS))
                .ToImmutableList();
        }

        private static IImmutableList<Field> PrepareProperties(LogEntry entry)
        {
            return entry.Properties?
                .Select(x => new Field($"{Propertiy}.{x.Name}", x.Value?.ToString(), Store.NO, ANALYZED_NO_NORMS))
                .ToImmutableList();
        }
    }
}