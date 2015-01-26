namespace NBlast.Storage.Tests.FileSystem

open System
open System.IO
open NBlast.Storage
open NBlast.Storage.Core
open NBlast.Storage.Core.Extensions
open NBlast.Storage.Core.Index
open NBlast.Storage.Core.Env
open NBlast.Storage.FileSystem
open Xunit
open FluentAssertions

[<AutoOpen>]
module StorageReaderExtensions = 
    type System.Object with
        member me.GenerateTempPath() = Path.Combine(Variables.TempFolderPath.Value, "NBlast_" + Guid.NewGuid().ToString())
        member me.MakeDirectoryProvider(path) = new ReaderDirectoryProvider(path)
        member me.MakeStorageReader(path, ?itemsPerPage) :IStorageReader =
            let directoryProvider = me.MakeDirectoryProvider(path)
            let paginator = new Paginator() :> IPaginator
            new StorageReader(directoryProvider, paginator, itemsPerPage |? 15) :> IStorageReader

        member me.MakeStorageWriter reopenWhenLocked path :IStorageWriter =
            let directoryProvider = new WriterDirectoryProvider(path, reopenWhenLocked)
            new StorageWriter(directoryProvider) :> IStorageWriter