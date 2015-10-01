using Lucene.Net.Documents;

namespace NBlast.Rest.Services.Write
{
    public interface IDocumentConverter<in T>: IConverter<T, Document>
    {
    }
}