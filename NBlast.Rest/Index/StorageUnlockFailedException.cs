using System.IO;

namespace NBlast.Rest.Index
{
    class StorageUnlockFailedException : IOException
    {
        public StorageUnlockFailedException(string path) 
            : base($"Unable to unlock index directory in {path}")
        {
        }
    }
}