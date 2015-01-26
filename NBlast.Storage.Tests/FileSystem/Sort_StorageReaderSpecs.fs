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
    member me.``It must realize a banal non-reversed sort by sender column``() =
        // Given

        // When
        // Then
        ignore ""

    member private this.``gimme 6 fake documents`` (?date: DateTime) =
        let date = DateTime.Now |> defaultArg date
        [ new LogDocument("senderA1", "1 C C", "log", "debug", None, date.AddDays(-1.0) |> Some);
          new LogDocument("senderA2", "2 B", "log", "debug", None, date.AddDays(-2.0) |> Some);
          new LogDocument("senderA3", "3 C", "log", "debug", None, date.AddDays(-3.0) |> Some);
          new LogDocument("senderB1", "4 BCD", "log", "debug", None, date.AddDays(-4.0) |> Some);
          new LogDocument("senderB2", "5 C", "log", "debug", None, date.AddDays(-5.0) |> Some);
          new LogDocument("senderB3", "6 B", "log", "debug", None, date.AddDays(-6.0) |> Some); ] 
            |> List.map (fun x -> x :> IStorageDocument)
