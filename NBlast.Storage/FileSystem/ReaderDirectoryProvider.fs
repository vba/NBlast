namespace NBlast.Storage.FileSystem

open Lucene.Net.Store
open NBlast.Storage.Core.Index
open NBlast.Storage.Core
open System.IO
open Lucene.Net.Store

type ReaderDirectoryProvider(path: string) = 
    interface IDirectoryProvider with
        member me.Provide () = 
            let directoryOpt = (me:>IDirectoryProvider).TryProvide()
            if (directoryOpt.IsNone) then raise(new NoSuchDirectoryException(path))
            else directoryOpt.Value

        member me.TryProvide () = 
            if Directory.Exists(path) 
                then FSDirectory.Open(new DirectoryInfo(path)) :> Lucene.Net.Store.Directory |> Some
                else None