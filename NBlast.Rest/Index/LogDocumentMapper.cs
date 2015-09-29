using System;
using System.Linq;
using Lucene.Net.Documents;
using Lucene.Net.Linq;
using Lucene.Net.Linq.Mapping;
using NBlast.Rest.Model.Write;
using static Lucene.Net.Util.Version;
using Version = Lucene.Net.Util.Version;

namespace NBlast.Rest.Index
{
    public class LogDocumentMapper: DocumentMapperBase<LogEntry>
    {
        private readonly IDocumentMapper<LogEntry> _initialMapper;

        public LogDocumentMapper(IDocumentMapper<LogEntry> initialMapper,
                                 Version version = LUCENE_30) : base(version)
        {
            if (initialMapper == null) throw new ArgumentNullException(nameof(initialMapper));
            _initialMapper = initialMapper;

            
        }

        public override void ToObject(Document source, IQueryExecutionContext context, LogEntry target)
        {
            base.ToObject(source, context, target);
        }

        public override void ToDocument(LogEntry source, Document target)
        {
            base.ToDocument(source, target);
        }
    }
}