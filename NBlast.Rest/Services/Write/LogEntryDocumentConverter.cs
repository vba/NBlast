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
            var document = new Document();

            document.Add(new Field(nameof(LogEntry.Id), entry.Id.ToString(), YES, NOT_ANALYZED_NO_NORMS));
            document.Add(new Field(nameof(LogEntry.Level), entry.Level, YES, ANALYZED_NO_NORMS));
            document.Add(new Field(nameof(LogEntry.Content), entry.Content, Store.NO, ANALYZED_NO_NORMS));
            document.Add(new Field(nameof(LogEntry.Data), entry.Data, YES, NOT_ANALYZED_NO_NORMS));
            document.Add(new Field(Type, typeof(LogEntry).Name, YES, NOT_ANALYZED_NO_NORMS));
            document.Add(new Field(nameof(LogEntry.CreationDate), DateTools.DateToString(entry.CreationDate, DateTools.Resolution.SECOND), Store.NO, ANALYZED_NO_NORMS));

            if (!IsNullOrEmpty(entry.Exception))
            {
                document.Add(new Field(nameof(LogEntry.Exception), entry.Exception, YES, ANALYZED_NO_NORMS));
            }

            PrepareProperties(entry)
                .Union(PrepareTemplateTexts(entry))
                .Union(PrepareTemplateProperties(entry))
                .ToImmutableList()
                .ForEach(x => document.Add(x));

            return document;
        }

        private static ImmutableList<Field> PrepareTemplateProperties(LogEntry entry)
        {
            return entry.TemplateTokensProperties?
                .Select(x => new Field($"{TemplateTokensProperty}", x.Item2, Store.NO, ANALYZED_NO_NORMS))
                .ToImmutableList();
        }
        private static ImmutableList<Field> PrepareTemplateTexts(LogEntry entry)
        {
            return entry.TemplateTokensTexts?
                .Select(x => new Field($"{TemplateTokensText}", x.Item2, Store.NO, ANALYZED_NO_NORMS))
                .ToImmutableList();
        }

        private static ImmutableList<Field> PrepareProperties(LogEntry entry)
        {
            return entry.Properties?
                .Select(x => new Field($"{Propertiy}.{x.Item1}", x.Item2?.ToString(), Store.NO, ANALYZED_NO_NORMS))
                .ToImmutableList();
        }
    }
}