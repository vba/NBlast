using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Lucene.Net.Linq;
using Lucene.Net.Linq.Abstractions;
using Lucene.Net.Linq.Mapping;
using static System.Threading.LazyThreadSafetyMode;
using LinqLuceneDataProvider = Lucene.Net.Linq.LuceneDataProvider;
using Version = Lucene.Net.Util.Version;

namespace NBlast.Rest.Index
{
    [ExcludeFromCodeCoverage]
    public class LuceneDataProvider : ILuceneDataProvider
    {
        private readonly Lazy<LinqLuceneDataProvider> _luceneDataProvider;

        public LuceneDataProvider(IDirectoryProvider directoryProvider)
        {
            _luceneDataProvider = new Lazy<LinqLuceneDataProvider>(
                () => new LinqLuceneDataProvider(directoryProvider.Provide(), Version.LUCENE_30),
                PublicationOnly
            );
        }

        public FieldMappingQueryParser<T> CreateQueryParser<T>()
        {
            return _luceneDataProvider.Value.CreateQueryParser<T>();
        }

        public IQueryable<T> AsQueryable<T>() where T : new()
        {
            return _luceneDataProvider.Value.AsQueryable<T>();
        }

        public IQueryable<T> AsQueryable<T>(IDocumentMapper<T> documentMapper) where T : new()
        {
            return _luceneDataProvider.Value.AsQueryable(documentMapper);
        }

        public IQueryable<T> AsQueryable<T>(ObjectFactory<T> factory)
        {
            return _luceneDataProvider.Value.AsQueryable(factory);
        }

        public IQueryable<T> AsQueryable<T>(ObjectLookup<T> lookup)
        {
            return _luceneDataProvider.Value.AsQueryable(lookup);
        }

        public IQueryable<T> AsQueryable<T>(ObjectFactory<T> factory, IDocumentMapper<T> documentMapper)
        {
            return _luceneDataProvider.Value.AsQueryable(factory, documentMapper);
        }

        public IQueryable<T> AsQueryable<T>(ObjectLookup<T> lookup, IDocumentMapper<T> documentMapper)
        {
            return _luceneDataProvider.Value.AsQueryable(lookup, documentMapper);
        }

        public IEnumerable<string> GetIndexedPropertyNames<T>()
        {
            return _luceneDataProvider.Value.GetIndexedPropertyNames<T>();
        }

        public ISession<T> OpenSession<T>() where T : new()
        {
            return _luceneDataProvider.Value.OpenSession<T>();
        }

        public ISession<T> OpenSession<T>(IDocumentMapper<T> documentMapper) where T : new()
        {
            return _luceneDataProvider.Value.OpenSession(documentMapper);
        }

        public ISession<T> OpenSession<T>(ObjectFactory<T> factory)
        {
            return _luceneDataProvider.Value.OpenSession(factory);
        }

        public ISession<T> OpenSession<T>(ObjectLookup<T> lookup)
        {
            return _luceneDataProvider.Value.OpenSession(lookup);
        }

        public ISession<T> OpenSession<T>(ObjectFactory<T> factory, IDocumentMapper<T> documentMapper)
        {
            return _luceneDataProvider.Value.OpenSession(factory, documentMapper);
        }

        public ISession<T> OpenSession<T>(ObjectLookup<T> lookup, IDocumentMapper<T> documentMapper)
        {
            return _luceneDataProvider.Value.OpenSession(lookup, documentMapper);
        }

        public ISession<T> OpenSession<T>(ObjectFactory<T> factory, IDocumentMapper<T> documentMapper, IDocumentModificationDetector<T> documentModificationDetector)
        {
            return _luceneDataProvider.Value.OpenSession(factory, documentMapper, documentModificationDetector);
        }

        public ISession<T> OpenSession<T>(ObjectLookup<T> lookup, IDocumentMapper<T> documentMapper, IDocumentModificationDetector<T> documentModificationDetector)
        {
            return _luceneDataProvider.Value.OpenSession(lookup, documentMapper, documentModificationDetector);
        }

        public void Dispose()
        {
            _luceneDataProvider.Value.Dispose();
        }

        public LuceneDataProviderSettings Settings
        {
            get { return _luceneDataProvider.Value.Settings; }
            set { _luceneDataProvider.Value.Settings = value; }
        }

        public Version LuceneVersion => _luceneDataProvider.Value.LuceneVersion;

        public IIndexWriter IndexWriter => _luceneDataProvider.Value.IndexWriter;
    }
}