using System;
using System.Collections.Immutable;
using System.Linq;
using NBlast.Rest.Index;
using NBlast.Rest.Model.Read;

namespace NBlast.Rest.Services.Read
{
    public class StandardSearchService : IStandardSearchService
    {
        private readonly ILogHitMapperProvider _mapperProvider;
        private readonly ILuceneDataProvider _luceneDataProvider;

        public StandardSearchService(ILogHitMapperProvider mapperProvider,
                                     ILuceneDataProvider luceneDataProvider)
        {
            if (mapperProvider == null) throw new ArgumentNullException(nameof(mapperProvider));
            if (luceneDataProvider == null) throw new ArgumentNullException(nameof(luceneDataProvider));
            _mapperProvider = mapperProvider;
            _luceneDataProvider = luceneDataProvider;
        }

        public LogHits SearchContent (string query, int skip = 0, int take = 20)
        {
            
            var hits = _luceneDataProvider.AsQueryable(_mapperProvider.Provide()).Where(x => x.Content == query);
            return new LogHits(0L, hits.Count(), hits.Skip(skip).Take(take).ToImmutableList());
            
        }
        
    }
}