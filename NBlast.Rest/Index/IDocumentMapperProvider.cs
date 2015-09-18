using Lucene.Net.Linq.Mapping;

namespace NBlast.Rest.Index
{
    public interface IDocumentMapperProvider<in T>
    {
        IDocumentMapper<T> Provide(); 
    }
}