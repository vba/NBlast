using Lucene.Net.Documents;
using NBlast.Rest.Model.Write;
using static System.String;
using static Lucene.Net.Documents.Field.Index;
using static Lucene.Net.Documents.Field.Store;
using static Lucene.Net.Documents.Field;

namespace NBlast.Rest.Services.Write
{
    public class LogEntryDocumentConverter
    {
        public Document Convert(LogEntry entry)
        {
            var document = new Document();

            document.Add(new Field(nameof(LogEntry.Id), entry.Id.ToString(), YES, NOT_ANALYZED_NO_NORMS));
            document.Add(new Field(nameof(LogEntry.Level), entry.Level, YES, ANALYZED_NO_NORMS));
            document.Add(new Field(nameof(LogEntry.Content), entry.Content, Store.NO, ANALYZED_NO_NORMS));
            document.Add(new Field(nameof(LogEntry.Data), entry.Data, YES, NOT_ANALYZED_NO_NORMS));
            document.Add(new Field("Type", typeof(LogEntry).Name, YES, NOT_ANALYZED_NO_NORMS));

            if (!IsNullOrEmpty(entry.Exception))
            {
                document.Add(new Field(nameof(LogEntry.Exception), entry.Exception, YES, ANALYZED_NO_NORMS));
            }

            return document;
        }
    }
}