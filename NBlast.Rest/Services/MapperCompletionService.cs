using System;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Linq.Mapping;
using NBlast.Rest.Model.Write;
using Remotion.Linq.Clauses;

namespace NBlast.Rest.Services
{
    public class MapperCompletionService<T> where T : LogEntry
    {
        private readonly ILogEntryMapperProvider _logEntryMapperProvider;

        public MapperCompletionService(ILogEntryMapperProvider logEntryMapperProvider)
        {
            if (logEntryMapperProvider == null) throw new ArgumentNullException(nameof(logEntryMapperProvider));
            _logEntryMapperProvider = logEntryMapperProvider;
        }

        public IDocumentMapper<T> Complete(params T[] entries)
        {
            var mapper = _logEntryMapperProvider.Provide() as DocumentMapperBase<T>;
            if (mapper == null)
            {
                throw new InvalidOperationException("Invalid mapper type");
            }

            //TODO: mapper.AddField();

            return mapper;
        }
    }
}