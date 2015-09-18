using LanguageExt;
using Lucene.Net.Store;

namespace NBlast.Rest.Index
{
    public interface IDirectoryProvider
    {
        Directory Provide();
        Option<Directory> TryProvide();
    }
}