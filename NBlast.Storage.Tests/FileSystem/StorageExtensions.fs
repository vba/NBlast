namespace NBlast.Storage.Tests.FileSystem

open System
open System.IO
open Lucene.Net.Store
open NBlast.Storage
open NBlast.Storage.Core
open NBlast.Storage.Core.Extensions
open NBlast.Storage.Core.Index
open NBlast.Storage.Core.Env
open NBlast.Storage.FileSystem
open NUnit.Framework
open Moq
open FluentAssertions

[<AutoOpen>]
module StorageReaderExtensions = 
    type System.Object with
        member me.GenerateTempPath() = Path.Combine(Variables.TempFolderPath.Value, "NBlast_" + Guid.NewGuid().ToString())
        member me.MakeDirectoryProvider path = //new ReaderDirectoryProvider(path)
            let provider = new Mock<IDirectoryProvider>(MockBehavior.Strict)
            provider
                .Setup(fun x -> x.Provide())
                .Returns(fun () -> FSDirectory.Open(new DirectoryInfo(path)) :> Lucene.Net.Store.Directory) |> ignore

            provider
                .Setup(fun x -> x.TryProvide())
                .Returns(fun () -> FSDirectory.Open(new DirectoryInfo(path)) :> Lucene.Net.Store.Directory |> Some) |> ignore

            provider.Object

        member me.MakeStorageReader(path, ?itemsPerPage) :IStorageReader =
            let directoryProvider = me.MakeDirectoryProvider(path)
            let paginator = new Paginator() :> IPaginator
            new StorageReader(directoryProvider, paginator, itemsPerPage |? 15) :> IStorageReader

        member me.MakeStorageWriter reopenWhenLocked path :IStorageWriter =
            let directoryProvider = new WriterDirectoryProvider(path, reopenWhenLocked)
            new StorageWriter(directoryProvider) :> IStorageWriter