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
open FluentAssertions


type TermsSearch_StorageReaderSpecs() =
    [<Test>]
    member this.``Reader should get no results when term search conditions are not respected``() =
        // Given
        let path = this.GenerateTempPath()
        let writer = new StorageWriter(this.MakeDirectoryProvider(path)) :> IStorageWriter
        this.``gimme 7 fake documents``() |> Seq.iter writer.InsertOne
        let sut = this.MakeStorageReader path
        let query = SearchQuery.GetOnlyExpression  "sender1"

        // When
        let actuals = (sut.SearchByTerm (LogField.Sender, query)).Hits 
        
        // Then
        (actuals |> List.length).Should().Be(0, "No document must be found") |> ignore

    [<Test>]
    member this.``Reader should accomplish a one-result banal search based on term query``() =
        // Given
        let path = this.GenerateTempPath()
        let writer = new StorageWriter(this.MakeDirectoryProvider(path)) :> IStorageWriter
        this.``gimme 7 fake documents``() |> Seq.iter writer.InsertOne
        let sut = this.MakeStorageReader path
        let query = SearchQuery.GetOnlyExpression  "sender1 sender6"

        // When
        let actuals = (sut.SearchByTerm (LogField.Sender, query)).Hits 
        
        // Then
        (actuals |> List.length).Should().Be(1, "Only 1 document must be found") |> ignore
        actuals.[0].Sender.Should().Be(query.Expression, "Term value must be equal to expression") |> ignore

    [<Test>]
    member this.``Reader should accomplish a multi-result banal search based on term query``() =
        // Given
        let path = this.GenerateTempPath()
        let writer = new StorageWriter(this.MakeDirectoryProvider(path)) :> IStorageWriter
        this.``gimme 7 fake documents``() |> Seq.iter writer.InsertOne
        let sut = this.MakeStorageReader path
        let query = SearchQuery.GetOnlyExpression  "sender6 sender1"

        // When
        let actuals = (sut.SearchByTerm (LogField.Sender, query)).Hits 
        
        // Then
        (actuals |> List.length).Should().Be(2, "Only 2 documents must be found") |> ignore
        [
            actuals.[0].Sender.Should().Be(query.Expression, "Term value must be equal to expression")
            actuals.[1].Sender.Should().Be(query.Expression, "Term value must be equal to expression")
        ] |> ignore

    [<Test>]
    member this.``Reader should accomplish terms search in the couple with filters/sort parts``() =
        // Given
        let path = this.GenerateTempPath()
        let date = DateTime.Now
        let writer = new StorageWriter(this.MakeDirectoryProvider(path)) :> IStorageWriter
        this.``gimme 7 fake documents``(date) |> Seq.iter writer.InsertOne
        let sut = this.MakeStorageReader path
        let query = SearchQuery.GetOnlyExpression  "sender6 sender1"
        let query = {
            SearchQuery.GetOnlyExpression("sender6 sender1") with 
                Filter = FilterQuery.Between(date.AddDays(-8.0), date.AddDays(-5.0)) |> Some
                Sort = {Field = LogField.Logger; Reverse = true} |> Some
        }

        // When
        let actuals = (sut.SearchByTerm (LogField.Sender, query)).Hits 
        
        // Then
        (actuals |> List.length).Should().Be(2, "Only 2 documents must be found") |> ignore
        [
            actuals.[0].Sender.Should().Be(query.Expression, "Term value must be equal to expression")
            actuals.[0].Logger.Should().Be("ulog2", "Sort reversion must be respected")
            actuals.[1].Sender.Should().Be(query.Expression, "Term value must be equal to expression")
            actuals.[1].Logger.Should().Be("ulog1", "Sort reversion must be respected")
        ] |> ignore


    member private this.``gimme 7 fake documents`` (?date: DateTime) =
        let date = DateTime.Now |> defaultArg date
        [ new LogDocument("sender1 sender6", "1 C C", "log", "debug", None, date.AddDays(-1.0) |> Some);
          new LogDocument("sender2 sender5", "2 B", "log", "debug", None, date.AddDays(-2.0) |> Some);
          new LogDocument("sender3 sender4", "3 C", "log", "debug", None, date.AddDays(-3.0) |> Some);
          new LogDocument("sender4 sender3", "4 BCD", "log", "debug", None, date.AddDays(-4.0) |> Some);
          new LogDocument("sender5 sender2", "5 C", "log", "debug", None, date.AddDays(-5.0) |> Some);
          new LogDocument("sender6 sender1", "6 B", "ulog1", "debug", None, date.AddDays(-6.0) |> Some); 
          new LogDocument("sender6 sender1", "6 Bis", "ulog2", "debug", None, date.AddDays(-7.0) |> Some); ] 
            |> List.map (fun x -> x :> IStorageDocument)
