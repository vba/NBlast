using System.IO;

namespace NBlast.Rest.Index
{
    class StorageLockedException : IOException
    {
        public StorageLockedException(string path) 
            : base($"Index directory is already locked in {path}")
        {
        }
    }
}