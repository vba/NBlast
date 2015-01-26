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


type Sort_StorageReaderSpecs() = 

    [<Fact>]
    member me.``Reader must realize a banal non-reversed sort by sender column``() =
        // Given
        let path = me.GenerateTempPath()
        let writer = new StorageWriter(me.MakeDirectoryProvider(path)) :> IStorageWriter
        let sut = me.MakeStorageReader(path)
        let date = DateTime.Now
        let query = {
            SearchQuery.GetOnlyExpression() with 
                Sort = LogField.Sender |> (Sort.OnlyField >> Some)
        }
        // When
        me.``gimme 6 fake documents``(date) |> writer.InsertMany
        let hits = sut.SearchByField query

        // Then
        hits.Total.Should().Be(6, "Should return 6 hits") |> ignore
        [
            hits.Hits.[0].Sender.Should().Be("senderA1", "senderA1 expected")
            hits.Hits.[1].Sender.Should().Be("senderA2", "senderA2 expected")
            hits.Hits.[2].Sender.Should().Be("senderA3", "senderA3 expected")
            hits.Hits.[3].Sender.Should().Be("senderB1", "senderB1 expected")
            hits.Hits.[4].Sender.Should().Be("senderB2", "senderB2 expected")
            hits.Hits.[5].Sender.Should().Be("senderB3", "senderB3 expected")
        ] |> ignore

    [<Fact>]
    member me.``Reader must realize a banal reversed sort by sender column``() =
        // Given
        let path = me.GenerateTempPath()
        let writer = new StorageWriter(me.MakeDirectoryProvider(path)) :> IStorageWriter
        let sut = me.MakeStorageReader(path)
        let date = DateTime.Now
        let query = {
            SearchQuery.GetOnlyExpression() with 
                Sort = { Field = LogField.Sender; Reverse = true } |> Some
        }
        // When
        me.``gimme 6 fake documents``(date) |> writer.InsertMany
        let hits = sut.SearchByField query

        // Then
        hits.Total.Should().Be(6, "Should return 6 hits") |> ignore
        [
            hits.Hits.[0].Sender.Should().Be("senderB3", "senderB3 expected")
            hits.Hits.[1].Sender.Should().Be("senderB2", "senderB2 expected")
            hits.Hits.[2].Sender.Should().Be("senderB1", "senderB1 expected")
            hits.Hits.[3].Sender.Should().Be("senderA3", "senderA3 expected")
            hits.Hits.[4].Sender.Should().Be("senderA2", "senderA2 expected")
            hits.Hits.[5].Sender.Should().Be("senderA1", "senderA1 expected")
        ] |> ignore

    member private this.``gimme 6 fake documents`` (?date: DateTime) =
        let date = DateTime.Now |> defaultArg date
        [ new LogDocument("senderA1", "1 C C", "log", "debug", None, date.AddDays(-1.0) |> Some);
          new LogDocument("senderB2", "5 C", "log", "debug", None, date.AddDays(-5.0) |> Some);
          new LogDocument("senderA3", "3 C", "log", "debug", None, date.AddDays(-3.0) |> Some);
          new LogDocument("senderB1", "4 BCD", "log", "debug", None, date.AddDays(-4.0) |> Some);
          new LogDocument("senderA2", "2 B", "log", "debug", None, date.AddDays(-2.0) |> Some);
          new LogDocument("senderB3", "6 B", "log", "debug", None, date.AddDays(-6.0) |> Some); ] 
            |> List.map (fun x -> x :> IStorageDocument)
