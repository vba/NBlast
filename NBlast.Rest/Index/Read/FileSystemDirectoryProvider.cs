using System;
using System.IO;
using LanguageExt;
using Lucene.Net.Store;
using static LanguageExt.Prelude;
using Directory = Lucene.Net.Store.Directory;

namespace NBlast.Rest.Index.Read
{
    public class FileSystemDirectoryProvider: IDirectoryProvider
    {
        private readonly string _indexPath;

        public FileSystemDirectoryProvider(string indexPath)
        {
            if (indexPath == null) throw new ArgumentNullException(nameof(indexPath));
            _indexPath = indexPath;
        }

        public Directory Provide()
        {
            return TryProvide().Match(
                Some: d => d,
                None: () => {
                    throw new NoSuchDirectoryException(_indexPath);
                }
            );
        }

        public Option<Directory> TryProvide()
        {
            return (System.IO.Directory.Exists(_indexPath))
                ? Some(FSDirectory.Open(new DirectoryInfo(_indexPath)) as Directory)
                : None;
        }
    }
}