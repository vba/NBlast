using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Lucene.Net.Documents;
using NBlast.Rest.Model.Write;
using NBlast.Rest.Tools;
using static System.String;
using static Lucene.Net.Documents.DateTools;
using static Lucene.Net.Documents.Field.Index;
using static Lucene.Net.Documents.Field.Store;
using static Lucene.Net.Documents.Field;
using static NBlast.Rest.Services.ServiceConstant.FieldNames;

namespace NBlast.Rest.Services.Write
{
    public class LogEntryDocumentConverter : IDocumentConverter<LogEntry>
    {
        public Document Convert(LogEntry logModel)
        {
            var document = StartBuildDocument(logModel);

            PrepareProperties(logModel)
                .Union(PrepareTemplateTexts(logModel))
                .Union(PrepareTemplateProperties(logModel))
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
            document.Add(new Field(ServiceConstant.FieldNames.Type, typeof (LogEntry).Name, YES, NOT_ANALYZED_NO_NORMS));
            document.Add(new Field(nameof(LogEntry.CreationDate), DateToString(entry.CreationDate, Resolution.SECOND), Store.NO, ANALYZED_NO_NORMS));

            if (!IsNullOrEmpty(entry.Exception))
            {
                document.Add(new Field(nameof(LogEntry.Exception), entry.Exception, YES, ANALYZED_NO_NORMS));
            }
            return document;
        }

        private static IImmutableList<Field> PrepareTemplateProperties(LogEntry entry)
        {
            return entry.TemplateTokensProperties?
                .Select(x => new Field(TemplateTokensProperty, x, Store.NO, ANALYZED_NO_NORMS))
                .ToImmutableList();
        }
        private static IImmutableList<Field> PrepareTemplateTexts(LogEntry entry)
        {
            return entry.TemplateTokensTexts?
                .Select(x => new Field(TemplateTokensText, x, Store.NO, ANALYZED_NO_NORMS))
                .ToImmutableList();
        }

        private static IImmutableList<IFieldable> PrepareProperties(LogEntry entry)
        {
            return entry.Properties?
                .Select(x =>
                {
                    var name = $"{Propertiy}.{x.Name}";

                    if (x.Value is DateTime) // TODO add check for datetime offsets
                    {
                        return new Field(name, DateToString(entry.CreationDate, Resolution.SECOND), Store.NO, ANALYZED_NO_NORMS) as IFieldable;
                    }
                    return (x.Value.IsNumber() && !x.Value.IsDecimal())
                        ? MakeNumericField(name, x) 
                        : new Field(name, x.Value?.ToString(), Store.NO, ANALYZED_NO_NORMS);
                })
                .ToImmutableList();
        }

        private static IFieldable MakeNumericField(string name, LogEntryProperty property)
        {
            var field = new NumericField(name, Store.NO, true);

            if (property.Value.IsLong())
            {
                field.SetLongValue((long) property.Value);
            }
            else if (property.Value.IsDouble())
            {
                field.SetDoubleValue((double) property.Value);
            }
            else if (property.Value.IsFloat())
            {
                field.SetFloatValue((float) property.Value);
            }
            else
            {
                field.SetIntValue((int) property.Value);
            }
            return field;
        }
    }
}