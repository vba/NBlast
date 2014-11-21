namespace NBlast.Storage.FileSystem

open Lucene.Net.Search
open Lucene.Net.Store
open Lucene.Net.Util
open Lucene.Net.Index
open NBlast.Storage.Core.Index
open NBlast.Storage.Core
open NBlast.Storage.Core.Exceptions
open System.IO

type WriterDirectoryProvider(path: string, ?reopenWhenLockedOp: bool) =
    
    interface IDirectoryProvider with
        member me.Provide () = 
            let openIndex = fun x -> FSDirectory.Open(new DirectoryInfo(x))
            let result = openIndex path
            let isLocked = IndexWriter.IsLocked(result)
            let reopenWhenLocked = if (reopenWhenLockedOp.IsSome) 
                                    then reopenWhenLockedOp.Value 
                                    else false

            if (Directory.Exists(path)) 
                then Directory.CreateDirectory(path) |> ignore

            if (reopenWhenLocked && isLocked) then
                try
                    result.ClearLock(IndexWriter.WRITE_LOCK_NAME) 
                with :? System.IO.IOException | :? LockObtainFailedException -> 
                    raise(new StorageUnlockFailedException(result.Directory.FullName))
            else if (isLocked) then
                raise(new StorageLockedException(result.Directory.FullName))
            result :> Lucene.Net.Store.Directory

