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
open Xunit
open FluentAssertions


type StorageReaderTest() = 

    [<Fact>]
    member this.``Reader must work as expected in the case of a banal search by field``() =
        // Given
        let path = Path.Combine(Variables.TempFolderPath.Value, Guid.NewGuid().ToString())
        let writer = new StorageWriter(path) :> IStorageWriter
        this.``gimme 6 fake documents``() |> Seq.iter writer.InsertOne
        let sut = this.MakeSut path

        // When
        let actuals = (sut.SearchByField "content:c" None None).Hits 

        // Then
        (actuals |> List.length).Should().Be(3, "Only 3 documents must be found") |> ignore
        actuals |> Seq.iter (fun actual -> 
            actual.Message.EndsWith("C").Should().BeTrue("Message must end with C") |> ignore
            actual.Sender.Should().Be("sender", "Sender value must be returned") |> ignore
            actual.Logger.Should().Be("log", "Logger value must be returned") |> ignore
            actual.Level.Should().Be("debug", "Debug value must be returned") |> ignore
            actual.CreatedAt.Should().BeBefore(DateTime.Now, "created at must respect specified interval") |> ignore
            actual.CreatedAt.Should().BeAfter(DateTime.Now.AddMinutes(-2.), "created at must respect specified interval") |> ignore
            actual.Score.IsNone.Should().BeFalse("Score must be present") |> ignore
        )

    [<Fact>]
    member this.``Reader must find and paginate results in the case of a banal search``() =
        // Given
        let path = Path.Combine(Variables.TempFolderPath.Value, Guid.NewGuid().ToString())
        let writer = new StorageWriter(path) :> IStorageWriter
        this.``gimme 15 fake documents``() |> Seq.iter writer.InsertOne
        let sut = this.MakeSut (path, 5)
        
        // When
        let actuals = (sut.SearchByField "content:c" (Some 1) None).Hits 

        // Then
        (actuals |> List.length).Should().Be(5, "Only 5 documents must be found") |> ignore
        actuals.Head.Message.Should().Be("3 C", "First element must be skiped") |> ignore

    [<Fact>]
    member this.``Reader must find and paginate in the end of found result slot``() =
        // Given
        let path = Path.Combine(Variables.TempFolderPath.Value, Guid.NewGuid().ToString())
        let writer = new StorageWriter(path) :> IStorageWriter
        this.``gimme 15 fake documents``() |> Seq.iter writer.InsertOne
        let sut = this.MakeSut (path, 5)
        
        // When
        let actuals = (sut.SearchByField "level:debug" (Some 13) None).Hits 

        // Then
        (actuals |> List.length).Should().Be(2, "Only 2 documents must be found") |> ignore
        actuals.Head.Message.Should().Be("14 B", "First elements must be skiped") |> ignore

    [<Fact>]
    member this.``Reader must find and paginate in the middle of found result slot``() =
        // Given
        let path = Path.Combine(Variables.TempFolderPath.Value, Guid.NewGuid().ToString())
        let writer = new StorageWriter(path) :> IStorageWriter
        this.``gimme 15 fake documents``() |> Seq.iter writer.InsertOne
        let sut = this.MakeSut (path, 5)
        
        // When
        let actuals = (sut.SearchByField "level:debug" (Some 10) None).Hits

        // Then
        (actuals |> List.length).Should().Be(5, "5 documents must be found") |> ignore
        actuals.Head.Message.Should().Be("11 C", "First element must be as expected") |> ignore
        (actuals |> List.rev).Head.Message.Should().Be("15 C", "Last element must be as expected") |> ignore

    [<Fact>]
    member this.``Reader must find all when it's requested``() =
        // Given
        let path = Path.Combine(Variables.TempFolderPath.Value, Guid.NewGuid().ToString())
        let writer = new StorageWriter(path) :> IStorageWriter
        this.``gimme 15 fake documents``() |> Seq.iter writer.InsertOne
        let sut = this.MakeSut (path, 5)
        
        // When
        let actuals = (sut.FindAll None (Some Int32.MaxValue)).Hits 

        // Then
        (actuals |> List.length).Should().Be(15, "15 documents must be found") |> ignore

    member private this.``gimme 6 fake documents`` () = 
        [ new LogDocument("sender", "1 C C", "log", "debug");
          new LogDocument("sender", "2 B", "log", "debug");
          new LogDocument("sender", "3 C", "log", "debug");
          new LogDocument("sender", "4 B", "log", "debug");
          new LogDocument("sender", "5 C", "log", "debug");
          new LogDocument("sender", "6 B", "log", "debug"); ] 
            |> List.map (fun x -> x :> IStorageDocument)

    member private this.``gimme 15 fake documents`` () = 
        [ new LogDocument("sender", "1 C C", "log", "debug");
          new LogDocument("sender", "2 B", "log", "debug");
          new LogDocument("sender", "3 C", "log", "debug");
          new LogDocument("sender", "4 B", "log", "debug");
          new LogDocument("sender", "5 C", "log", "debug");
          new LogDocument("sender", "6 B", "log", "debug"); 
          new LogDocument("sender", "7 C", "log", "debug"); 
          new LogDocument("sender", "8 B", "log", "debug"); 
          new LogDocument("sender", "9 C", "log", "debug"); 
          new LogDocument("sender", "10 B", "log", "debug"); 
          new LogDocument("sender", "11 C", "log", "debug"); 
          new LogDocument("sender", "12 B", "log", "debug"); 
          new LogDocument("sender", "13 C", "log", "debug"); 
          new LogDocument("sender", "14 B", "log", "debug"); 
          new LogDocument("sender", "15 C", "log", "debug"); ] 
            |> List.map (fun x -> x :> IStorageDocument)

    member private this.MakeSut(path, ?itemsPerPage) :IStorageReader =
        new StorageReader(path, itemsPerPage |? 15) :> IStorageReader