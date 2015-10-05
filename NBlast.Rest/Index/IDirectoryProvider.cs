using LanguageExt;
using Lucene.Net.Store;

namespace NBlast.Rest.Index
{

    public interface IDirectoryProvider<T> where T: Directory
    {
        T Provide();
        Option<T> TryProvide();
    }
    public interface IDirectoryProvider : IDirectoryProvider<Directory> { }

}