namespace NBlast.Storage.FileSystem

open Lucene.Net.Store
open NBlast.Storage.Core.Index
open NBlast.Storage.Core
open System.IO

type ReaderDirectoryProvider(path: string) = 
    interface IDirectoryProvider with
        member me.Provide () = 
            FSDirectory.Open(new DirectoryInfo(path)) :> Lucene.Net.Store.Directory