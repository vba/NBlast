using System;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Linq;
using Lucene.Net.Linq.Abstractions;
using Lucene.Net.Linq.Mapping;
using Version = Lucene.Net.Util.Version;

namespace NBlast.Rest.Index
{
    public interface ILuceneDataProvider: IDisposable
    {
        FieldMappingQueryParser<T> CreateQueryParser<T>();
        IQueryable<T> AsQueryable<T>() where T : new();
        IQueryable<T> AsQueryable<T>(IDocumentMapper<T> documentMapper) where T : new();
        IQueryable<T> AsQueryable<T>(ObjectFactory<T> factory);
        IQueryable<T> AsQueryable<T>(ObjectLookup<T> lookup);
        IQueryable<T> AsQueryable<T>(ObjectFactory<T> factory, IDocumentMapper<T> documentMapper);
        IQueryable<T> AsQueryable<T>(ObjectLookup<T> lookup, IDocumentMapper<T> documentMapper);
        IEnumerable<string> GetIndexedPropertyNames<T>();
        ISession<T> OpenSession<T>() where T : new();
        ISession<T> OpenSession<T>(IDocumentMapper<T> documentMapper) where T : new();
        ISession<T> OpenSession<T>(ObjectFactory<T> factory);
        ISession<T> OpenSession<T>(ObjectLookup<T> lookup);
        ISession<T> OpenSession<T>(ObjectFactory<T> factory, IDocumentMapper<T> documentMapper);
        ISession<T> OpenSession<T>(ObjectLookup<T> lookup, IDocumentMapper<T> documentMapper);
        ISession<T> OpenSession<T>(ObjectFactory<T> factory, IDocumentMapper<T> documentMapper, IDocumentModificationDetector<T> documentModificationDetector);
        ISession<T> OpenSession<T>(ObjectLookup<T> lookup, IDocumentMapper<T> documentMapper, IDocumentModificationDetector<T> documentModificationDetector);
        LuceneDataProviderSettings Settings { get; set; }
        Version LuceneVersion { get; }
        IIndexWriter IndexWriter { get; }
    }
}