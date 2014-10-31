namespace NBlast.Storage.Tests.FileSystem

open System
open System.Runtime
open Xunit
open FluentAssertions
open NBlast.Storage
open NBlast.Storage.FileSystem
open NBlast.Storage.Core.System
open NBlast.Storage.Core.Exceptions
open System.IO
open Lucene.Net.Store
open Lucene.Net.Util
open Lucene.Net.Index
open Lucene.Net.Analysis.Standard

type StorageWriterTest() = 

    [<Fact>]
    member this.``Writer directory must be created as expected``() =
        // Given
        let path = Path.Combine(Variables.TempFolderPath.Value, Guid.NewGuid().ToString())
        let sut  = (this.MakeSut false path) 

        // When
        sut.InsertOne()

        // Then
        (Directory.Exists(path)).Should().Be(true, sprintf "Storage has to create its directory in %s" path)

    [<Fact>]
    member this.``Writer directory creation must fail trying unlock locked directory``() =
        // Given
        let path = Path.Combine(Variables.TempFolderPath.Value, Guid.NewGuid().ToString())
        let sut  = (this.MakeSut true path) 
        let directory = FSDirectory.Open(new DirectoryInfo(path))

        // When
        Directory.CreateDirectory(path) |> ignore
        directory.MakeLock(IndexWriter.WRITE_LOCK_NAME).Obtain(0L) |> ignore
        directory.Dispose()

        Assert.Throws<StorageUnlockFailedException>(sut.InsertOne) 

    [<Fact>]
    member this.``Writer directory creation must fail when it's already locked``() =
        // Given
        let path = Path.Combine(Variables.TempFolderPath.Value, Guid.NewGuid().ToString())
        let sut  = (this.MakeSut false path) 
        let directory = FSDirectory.Open(new DirectoryInfo(path))

        // When
        Directory.CreateDirectory(path) |> ignore
        directory.MakeLock(IndexWriter.WRITE_LOCK_NAME).Obtain(0L) |> ignore
        directory.Dispose()

        Assert.Throws<StorageLockedException>(sut.InsertOne) //|> ignore

    member private this.MakeSut reopenWhenLocked path :StorageWriter =
        new StorageWriter(reopenWhenLocked, path)
        //result