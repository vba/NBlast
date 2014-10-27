namespace NBlast.Storage.Tests.FileSystem

open System
open System.Runtime
open Xunit
open FluentAssertions
open NBlast.Storage
open NBlast.Storage.Core.System
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
        let sut  = (this.MakeSut false path) :> StorageWriter 

        // When
        sut.InsertOne() //|> ignore

        // Then
        (Directory.Exists(path)).Should().Be(true, sprintf "Storage has to create its directory in %s" path)

    [<Fact>]
    member this.``Writer directory must be created and unlocked as expected``() =
        // Given
        let path = Path.Combine(Variables.TempFolderPath.Value, Guid.NewGuid().ToString())
        let directory = FSDirectory.Open(new DirectoryInfo(path))
        let analyser = new StandardAnalyzer(Version.LUCENE_30)
        let sut  = (this.MakeSut true path) :> StorageWriter 

        // When
        let writer = new IndexWriter(directory, analyser, IndexWriter.MaxFieldLength.UNLIMITED)
        writer = null |> ignore
        sut.InsertOne() //|> ignore

        // Then
        (Directory.Exists(path))
            .Should().Be(false, sprintf "Storage has to create and unlock its directory in %s" path) |> ignore
        (*
        (IndexWriter.IsLocked(directory))
            .Should().Be(false, sprintf "Directory should be unlocked in %s" path) |> ignore
*)

    
    member private this.MakeSut reopenWhenLocked path = 
        new StorageWriter(reopenWhenLocked, path)
        //result