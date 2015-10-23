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
    public class LogEventDocumentConverter : IDocumentConverter<LogEvent>
    {
        public Document Convert(LogEvent logModel)
        {
            var document = StartBuildDocument(logModel);

            PrepareProperties(logModel)
                .ToImmutableList()
                .ForEach(x => document.Add(x));

            return document;
        }

        private static Document StartBuildDocument(LogEvent @event)
        {
            var document = new Document();

            document.Add(new Field(nameof(LogEvent.Id), @event.Id.ToString(), YES, NOT_ANALYZED_NO_NORMS));
            document.Add(new Field(nameof(LogEvent.Level), @event.Level, YES, ANALYZED_NO_NORMS));
            document.Add(new Field("content", @event.GetContent(), Store.NO, ANALYZED_NO_NORMS));
            //document.Add(new Field(nameof(LogEvent.Data), @event.Data, YES, NOT_ANALYZED_NO_NORMS));
            document.Add(new Field(ServiceConstant.FieldNames.Type, typeof (LogEvent).Name, YES, NOT_ANALYZED_NO_NORMS));
            document.Add(new Field(nameof(LogEvent.Timestamp), DateToString(@event.Timestamp, Resolution.SECOND), Store.NO, ANALYZED_NO_NORMS));

            if (!IsNullOrEmpty(@event.Exception))
            {
                document.Add(new Field(nameof(LogEvent.Exception), @event.Exception, YES, ANALYZED_NO_NORMS));
            }
            return document;
        }

        private static IImmutableList<IFieldable> PrepareProperties(LogEvent entry)
        {
            return entry.Properties?
                .Select(x =>
                {
                    var name = $"{Propertiy}.{x.Key}";

                    if (x.Value is DateTime) // TODO add check for datetime offsets
                    {
                        return new Field(name, DateToString(entry.Timestamp, Resolution.SECOND), Store.NO, ANALYZED_NO_NORMS) as IFieldable;
                    }
                    return (x.Value.IsNumber() && !x.Value.IsDecimal())
                        ? MakeNumericField(name, x) 
                        : new Field(name, x.Value?.ToString(), Store.NO, ANALYZED_NO_NORMS);
                })
                .ToImmutableList();
        }

        private static IFieldable MakeNumericField(string name, LogEventProperty property)
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