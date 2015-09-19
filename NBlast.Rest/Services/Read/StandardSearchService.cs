using System;
using System.Collections.Immutable;
using System.Linq;
using Lucene.Net.Linq;
using NBlast.Rest.Configuration;
using NBlast.Rest.Index;
using NBlast.Rest.Model.Read;
using Ninject;
using static Lucene.Net.Util.Version;

namespace NBlast.Rest.Services.Read
{
    public class StandardSearchService : IStandardSearchService
    {
        private const string DirectoryProviderName = NinjectKernelSupplier.ReadDirectoryProviderName;
        private readonly ILogHitMapperProvider _mapperProvider;
        private readonly IDirectoryProvider _directoryProvider;

        public StandardSearchService(ILogHitMapperProvider mapperProvider,
                                     [Named(DirectoryProviderName)]IDirectoryProvider directoryProvider)
        {
            if (mapperProvider == null) throw new ArgumentNullException(nameof(mapperProvider));
            if (directoryProvider == null) throw new ArgumentNullException(nameof(directoryProvider));
            _mapperProvider = mapperProvider;
            _directoryProvider = directoryProvider;
        }

        public LogHits SearchContent (string query, int skip = 0, int take = 20)
        {
            using (var provider = BuildDataProvider())
            {
                var hits = provider.AsQueryable(_mapperProvider.Provide()).Where(x => x.Content == query);
                return new LogHits(0L, hits.Count(), hits.Skip(skip).Take(take).ToImmutableList());
            }
        }

        private LuceneDataProvider BuildDataProvider()
        {
            return new LuceneDataProvider(_directoryProvider.Provide(), LUCENE_30);
        }
    }
}