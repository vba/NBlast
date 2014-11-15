namespace NBlast.Storage.Tests.FileSystem

open System
open System.IO
open Lucene.Net.Store
open NBlast.Storage
open NBlast.Storage.Core
open NBlast.Storage.Core.Env
open NBlast.Storage.FileSystem
open Xunit
open FluentAssertions

type StorageReaderTest() = 

    [<Fact>]
    member this.``Reader must work as expected in the case of a banal search``() =
        // Given
        let path = Path.Combine(Variables.TempFolderPath.Value, Guid.NewGuid().ToString())
        let directory = FSDirectory.Open(new DirectoryInfo(path))
        let writer = new StorageWriter(path) :> IStorageWriter
        let logDocument = new LogDocument("sender", "1", "log", "debug", "", DateTime.Now)
        writer.InsertOne(logDocument :> IStorageDocument)

        //let sut  = (this.MakeSut false path)

        // When
//        Directory.CreateDirectory(path) |> ignore
//        directory.MakeLock(IndexWriter.WRITE_LOCK_NAME).Obtain(0L) |> ignore
//        directory.Dispose()


    member private this.MakeSut path :IStorageReader =
        new StorageReader(path) :> IStorageReader

