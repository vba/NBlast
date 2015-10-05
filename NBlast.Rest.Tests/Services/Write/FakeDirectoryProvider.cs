using LanguageExt;
using Lucene.Net.Store;
using NBlast.Rest.Index;

namespace NBlast.Rest.Tests.Services.Write
{
    internal class FakeDirectoryProvider: IDirectoryProvider<FakeRamDirectory>
    {

        public FakeRamDirectory Provide()
        {
            return TryProvide().Some(x => x).None(() => new FakeRamDirectory());
        }

        public Option<FakeRamDirectory> TryProvide()
        {
            return Prelude.Some(new FakeRamDirectory());
        }
    }
}