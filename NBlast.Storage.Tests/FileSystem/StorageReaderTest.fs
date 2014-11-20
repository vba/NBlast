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
        let writer = new StorageWriter(this.MakeDirectoryProvider(path)) :> IStorageWriter
        this.``gimme 6 fake documents``() |> Seq.iter writer.InsertOne
        let sut = this.MakeSut path

        // When
        let actuals = (sut.SearchByField (Content.QueryWith "c") None None).Hits 

        // Then
        (actuals |> List.length).Should().Be(3, "Only 3 documents must be found") |> ignore
        actuals |> Seq.iter (fun actual -> 
            actual.Message.EndsWith("C").Should().BeTrue("Message must end with C") |> ignore
            actual.Sender.Should().StartWith("sender", "Sender value should start with 'sender'") |> ignore
            actual.Logger.Should().Be("log", "Logger value must be returned") |> ignore
            actual.Level.Should().Be("debug", "Debug value must be returned") |> ignore
            actual.CreatedAt.Should().BeBefore(DateTime.Now, "created at must respect specified interval") |> ignore
            actual.CreatedAt.Should().BeAfter(DateTime.Now.AddMinutes(-2.), "created at must respect specified interval") |> ignore
            actual.Score.IsNone.Should().BeFalse("Score must be present") |> ignore
        )

    [<Fact>]
    member this.``Reader must search and collect expected amount of total documents``() =
        // Given
        let path = Path.Combine(Variables.TempFolderPath.Value, Guid.NewGuid().ToString())
        let writer = new StorageWriter(this.MakeDirectoryProvider(path)) :> IStorageWriter
        this.``gimme 15 fake documents``() |> Seq.iter writer.InsertOne
        this.``gimme 15 fake documents``() |> Seq.iter writer.InsertOne
        let sut = this.MakeSut (path, 5)
        
        // When
        let actuals = (sut.SearchByField (Content.QueryWith "b") (Some 0) (Some 1))

        // Then
        (actuals.Hits |> List.length).Should().Be(1, "Only 1 documents must be returned") |> ignore
        actuals.Total.Should().Be(14, "Total must be equal to 7*2") |> ignore

    [<Fact>]
    member this.``Reader must count all documents as expected``() =
        // Given
        let path = Path.Combine(Variables.TempFolderPath.Value, Guid.NewGuid().ToString())
        let writer = new StorageWriter(this.MakeDirectoryProvider(path)) :> IStorageWriter
        this.``gimme 15 fake documents``() |> Seq.iter writer.InsertOne
        this.``gimme 15 fake documents``() |> Seq.iter writer.InsertOne
        let sut = this.MakeSut (path, 5)
        
        // When
        let actualTotal = sut.CountAll()

        // Then
        actualTotal.Should().Be(30, "Total must be equal to 30") |> ignore

    [<Fact>]
    member this.``Reader must find and paginate results in the case of a banal search``() =
        // Given
        let path = Path.Combine(Variables.TempFolderPath.Value, Guid.NewGuid().ToString())
        let writer = new StorageWriter(this.MakeDirectoryProvider(path)) :> IStorageWriter
        this.``gimme 15 fake documents``() |> Seq.iter writer.InsertOne
        let sut = this.MakeSut (path, 5)
        
        // When
        let actuals = (sut.SearchByField (Content.QueryWith "c") (Some 1) None).Hits 

        // Then
        (actuals |> List.length).Should().Be(5, "Only 5 documents must be found") |> ignore
        actuals.Head.Message.Should().Be("3 C", "First element must be skiped") |> ignore

    [<Fact>]
    member this.``Reader must find and paginate in the end of found result slot``() =
        // Given
        let path = Path.Combine(Variables.TempFolderPath.Value, Guid.NewGuid().ToString())
        let writer = new StorageWriter(this.MakeDirectoryProvider(path)) :> IStorageWriter
        this.``gimme 15 fake documents``() |> Seq.iter writer.InsertOne
        let sut = this.MakeSut (path, 5)
        
        // When
        let actuals = (sut.SearchByField (Level.QueryWith "debug") (Some 13) None).Hits 

        // Then
        (actuals |> List.length).Should().Be(2, "Only 2 documents must be found") |> ignore
        actuals.Head.Message.Should().Be("14 B", "First elements must be skiped") |> ignore

    [<Fact>]
    member this.``Reader must find and paginate in the middle of found result slot``() =
        // Given
        let path = Path.Combine(Variables.TempFolderPath.Value, Guid.NewGuid().ToString())
        let writer = new StorageWriter(this.MakeDirectoryProvider(path)) :> IStorageWriter
        this.``gimme 15 fake documents``() |> Seq.iter writer.InsertOne
        let sut = this.MakeSut (path, 5)
        
        // When
        let actuals = (sut.SearchByField (Level.QueryWith "debug") (Some 10) None).Hits

        // Then
        (actuals |> List.length).Should().Be(5, "5 documents must be found") |> ignore
        actuals.Head.Message.Should().Be("11 C", "First element must be as expected") |> ignore
        (actuals |> List.rev).Head.Message.Should().Be("15 C", "Last element must be as expected") |> ignore

    [<Fact>]
    member this.``Reader must find all when it's requested``() =
        // Given
        let path = Path.Combine(Variables.TempFolderPath.Value, Guid.NewGuid().ToString())
        let writer = new StorageWriter(this.MakeDirectoryProvider(path)) :> IStorageWriter
        this.``gimme 15 fake documents``() |> Seq.iter writer.InsertOne
        let sut = this.MakeSut (path, 5)
        
        // When
        let actuals = (sut.FindAll None (Some Int32.MaxValue)).Hits 

        // Then
        (actuals |> List.length).Should().Be(15, "15 documents must be found") |> ignore

    [<Fact>]
    member this.``Reader must group index results by requested field as expected``() =
        // Given
        let path = Path.Combine(Variables.TempFolderPath.Value, Guid.NewGuid().ToString())
        let writer = new StorageWriter(this.MakeDirectoryProvider(path)) :> IStorageWriter
        this.``gimme 15 fake documents``() |> Seq.iter writer.InsertOne
        let sut = this.MakeSut (path, 5)
        
        // When
        let actualFacets = (sut.GroupWith LogField.Sender).Facets |> List.toArray

        // Then
        (actualFacets |> Array.length).Should().Be(3, "3 facet must be found") |> ignore
        actualFacets |> Seq.iteri (fun i x -> 
            x.Name.Should().StartWith("sender", "Facet name must start with sender") |> ignore
            x.Count.Should().Be(int64 5, "Each group must count 5 documents") |> ignore
        )


    member private this.``gimme 6 fake documents`` () = 
        [ new LogDocument("sender1", "1 C C", "log", "debug");
          new LogDocument("sender1", "2 B", "log", "debug");
          new LogDocument("sender1", "3 C", "log", "debug");
          new LogDocument("sender2", "4 B", "log", "debug");
          new LogDocument("sender2", "5 C", "log", "debug");
          new LogDocument("sender2", "6 B", "log", "debug"); ] 
            |> List.map (fun x -> x :> IStorageDocument)

    member private this.``gimme 15 fake documents`` () = 
        [ new LogDocument("sender1", "1 C C", "log", "debug");
          new LogDocument("sender1", "2 B", "log", "debug");
          new LogDocument("sender1", "3 C", "log", "debug");
          new LogDocument("sender1", "4 B", "log", "debug");
          new LogDocument("sender1", "5 C", "log", "debug");
          new LogDocument("sender2", "6 B", "log", "debug"); 
          new LogDocument("sender2", "7 C", "log", "debug"); 
          new LogDocument("sender2", "8 B", "log", "debug"); 
          new LogDocument("sender2", "9 C", "log", "debug"); 
          new LogDocument("sender2", "10 B", "log", "debug"); 
          new LogDocument("sender3", "11 C", "log", "debug"); 
          new LogDocument("sender3", "12 B", "log", "debug"); 
          new LogDocument("sender3", "13 C", "log", "debug"); 
          new LogDocument("sender3", "14 B", "log", "debug"); 
          new LogDocument("sender3", "15 C", "log", "debug"); ] 
            |> List.map (fun x -> x :> IStorageDocument)

    member private me.MakeDirectoryProvider(path) = new ReaderDirectoryProvider(path)

    member private this.MakeSut(path, ?itemsPerPage) :IStorageReader =
        let directoryProvider = this.MakeDirectoryProvider(path)
        
//        { 
//            new IDirectoryProvider with member me.Provide() = new RAMDirectory() :> Directory
//        }
        new StorageReader(directoryProvider, itemsPerPage |? 15) :> IStorageReader