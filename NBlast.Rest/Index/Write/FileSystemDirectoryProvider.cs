using System;
using System.IO;
using LanguageExt;
using static LanguageExt.Prelude;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Directory = Lucene.Net.Store.Directory;

namespace NBlast.Rest.Index.Write
{
    public class FileSystemDirectoryProvider : IDirectoryProvider
    {
        private readonly string _indexPath;
        private readonly bool _reopenWhenLocked;

        public FileSystemDirectoryProvider(string indexPath,
                                                bool reopenWhenLocked = false)
        {
            if (indexPath == null) throw new ArgumentNullException(nameof(indexPath));
            _indexPath = indexPath;
            _reopenWhenLocked = reopenWhenLocked;
        }

        public Option<Directory> TryProvide()
        {
            return Some(Provide());
        }

        public Directory Provide()
        {
            if (!System.IO.Directory.Exists(_indexPath))
            {
                System.IO.Directory.CreateDirectory(_indexPath);
            }
            var result = FSDirectory.Open(new DirectoryInfo(_indexPath));
            var isLocked = IndexWriter.IsLocked(result);


            return TryClearLock(isLocked, result);
        }

        private Directory TryClearLock(bool isLocked, FSDirectory result)
        {
            if (isLocked && _reopenWhenLocked)
            {
                try
                {
                    result.ClearLock(IndexWriter.WRITE_LOCK_NAME);
                }
                catch (Exception e) when (e is LockObtainFailedException || e is IOException)
                {
                    throw new StorageUnlockFailedException(result.Directory.FullName);
                }
            }
            else if (isLocked)
            {
                throw new StorageLockedException(result.Directory.FullName);
            }
            return result;
        }
    }
}