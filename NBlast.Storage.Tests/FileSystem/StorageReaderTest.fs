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
        let writer = new StorageWriter(path) :> IStorageWriter
        new LogDocument("sender", "1 C C", "log", "debug") :> IStorageDocument |> writer.InsertOne
        new LogDocument("sender", "2 B", "log", "debug") :> IStorageDocument |> writer.InsertOne
        new LogDocument("sender", "3 C", "log", "debug") :> IStorageDocument |> writer.InsertOne
        new LogDocument("sender", "4 B", "log", "debug") :> IStorageDocument |> writer.InsertOne
        new LogDocument("sender", "5 C", "log", "debug") :> IStorageDocument |> writer.InsertOne
        new LogDocument("sender", "6 B", "log", "debug") :> IStorageDocument |> writer.InsertOne
        let sut = this.MakeSut path

        // When
        let actuals = sut.Search "content" "c" None None

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


    member private this.MakeSut path :IStorageReader =
        new StorageReader(path) :> IStorageReader

