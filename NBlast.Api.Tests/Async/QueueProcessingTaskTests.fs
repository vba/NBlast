namespace NBlast.Api.Tests.Async

open NBlast.Api.Async
open NBlast.Api.Models
open NBlast.Storage.Core.Extensions
open NBlast.Storage
open NBlast.Storage.Core.Index
open NBlast.Storage.Core.Extensions
open System
open System.Linq.Expressions
open System.Runtime
open Xunit
open FluentAssertions
open Ploeh.AutoFixture
open Ploeh.AutoFixture.Kernel
open Moq
open FluentScheduler

type FakeStorageWriter() = 
    member val IndexedDocuments: LogDocument list = [] with get, set
    interface IStorageWriter with
        member me.InsertOne doc =
            me.IndexedDocuments <- (doc :?> LogDocument) :: me.IndexedDocuments
            ignore ""

type QueueProcessingTaskTests() =

    [<Fact>]
    member me.``Task execution must consume and convert model to appropriated entity without error``() =
        // Given
        let actualAmount = 20
        let fixture = new Fixture()
        let storageWriter = new FakeStorageWriter()
        let queueKeeper = new Mock<IIndexingQueueKeeper>(MockBehavior.Strict)
        let mutable collector: LogDocument list = List.empty

        queueKeeper
            .Setup<seq<LogModel>>(fun x -> x.ConsumeMany(Some actualAmount))
            .Returns(fun () -> 
                [0 .. actualAmount - 1] |> List.map (fun x -> 
                    new LogModel(Sender=fixture.Create(), 
                                 Message=fixture.Create(),
                                 Logger=fixture.Create(),
                                 Level=fixture.Create())
                ) |> List.toSeq
            ) |> ignore
        queueKeeper
            .Setup<int>(fun x -> x.Count()).Returns(fun () -> actualAmount) |> ignore
        
        let sut = me.MakeSut(queueKeeper.Object, storageWriter)

        // When
        sut.Execute()

        // Then
        queueKeeper.VerifyAll()
        storageWriter.IndexedDocuments.Length.Should().Be(actualAmount, "Indexer must capture 20 documents") |> ignore
        storageWriter.IndexedDocuments |> List.iter (fun x ->
            x.Error.IsNone.Should().BeTrue("Error field must be empty") |> ignore
            x.Sender.Value.Should().NotBeNullOrEmpty("Sender field must have a value") |> ignore
            x.Message.Value.Should().NotBeNullOrEmpty("Message field must have a value") |> ignore
            x.Logger.Value.Should().NotBeNullOrEmpty("Logger field must have a value") |> ignore
            x.Level.Value.Should().NotBeNullOrEmpty("Level field must have a value") |> ignore
            (x.CreatedAt.Value > DateTime.MinValue).Should().BeTrue("CreatedAt cannot be empty") |> ignore
            x.Content.Value.Should().Contain(x.Message.Value, "Content should contain message") |> ignore
        ) 


    [<Fact>]
    member me.``Task execution must consume and convert model to appropriated entity with current date``() =
        // Given
        let actualAmount = 20
        let fixture = new Fixture()
        let storageWriter = new FakeStorageWriter()
        let queueKeeper = new Mock<IIndexingQueueKeeper>(MockBehavior.Strict)
        let mutable collector: LogDocument list = List.empty

        queueKeeper
            .Setup<seq<LogModel>>(fun x -> x.ConsumeMany(Some actualAmount))
            .Returns(fun () -> 
                [0 .. actualAmount - 1] |> List.map (fun x -> 
                    new LogModel(Sender=fixture.Create(), 
                                 Message=fixture.Create(),
                                 Logger=fixture.Create(),
                                 Level=fixture.Create(),
                                 Error=fixture.Create(),
                                 CreatedAt=Unchecked.defaultof<_>)
                ) |> List.toSeq
            ) |> ignore
        queueKeeper
            .Setup<int>(fun x -> x.Count()).Returns(fun () -> actualAmount) |> ignore
        
        let sut = me.MakeSut(queueKeeper.Object, storageWriter)

        // When
        sut.Execute()

        // Then
        queueKeeper.VerifyAll()
        storageWriter.IndexedDocuments.Length.Should().Be(actualAmount, "Indexer must capture 20 documents") |> ignore
        storageWriter.IndexedDocuments |> List.iter (fun x ->
            x.Error.Value.Value.Should().NotBeNullOrEmpty("Error field must have a value") |> ignore
            x.Sender.Value.Should().NotBeNullOrEmpty("Sender field must have a value") |> ignore
            x.Message.Value.Should().NotBeNullOrEmpty("Message field must have a value") |> ignore
            x.Logger.Value.Should().NotBeNullOrEmpty("Logger field must have a value") |> ignore
            x.Level.Value.Should().NotBeNullOrEmpty("Level field must have a value") |> ignore
            (x.CreatedAt.Value > DateTime.MinValue).Should().BeTrue("CreatedAt cannot be empty") |> ignore
            x.Content.Value.Should().Contain(x.Error.Value.Value, "Content should contain error") |> ignore
            x.Content.Value.Should().Contain(x.Message.Value, "Content should contain message") |> ignore
        ) 

    [<Fact>]
    member me.``Task execution must consume and convert model to appropriated entity with all fields``() =
        // Given
        let actualAmount = 20
        let fixture = new Fixture()
        let expectedCreatedAt = new Nullable<DateTime>(new DateTime(1977, 01, 10))
        let storageWriter = new FakeStorageWriter()
        let queueKeeper = new Mock<IIndexingQueueKeeper>(MockBehavior.Strict)
        let mutable collector: LogDocument list = List.empty

        queueKeeper
            .Setup<seq<LogModel>>(fun x -> x.ConsumeMany(Some actualAmount))
            .Returns(fun () -> 
                [0 .. actualAmount - 1] |> List.map (fun x -> 
                    new LogModel(Sender=fixture.Create(), 
                                 Message=fixture.Create(),
                                 Logger=fixture.Create(),
                                 Level=fixture.Create(),
                                 Error=fixture.Create(),
                                 CreatedAt=expectedCreatedAt)
                ) |> List.toSeq
            ) |> ignore
        queueKeeper
            .Setup<int>(fun x -> x.Count()).Returns(fun () -> actualAmount) |> ignore
        
        let sut = me.MakeSut(queueKeeper.Object, storageWriter)

        // When
        sut.Execute()

        // Then
        queueKeeper.VerifyAll()
        storageWriter.IndexedDocuments.Length.Should().Be(actualAmount, "Indexer must capture 20 documents") |> ignore
        storageWriter.IndexedDocuments |> List.iter (fun x ->
            x.Error.Value.Value.Should().NotBeNullOrEmpty("Error field must have a value") |> ignore
            x.Sender.Value.Should().NotBeNullOrEmpty("Sender field must have a value") |> ignore
            x.Message.Value.Should().NotBeNullOrEmpty("Message field must have a value") |> ignore
            x.Logger.Value.Should().NotBeNullOrEmpty("Logger field must have a value") |> ignore
            x.Level.Value.Should().NotBeNullOrEmpty("Level field must have a value") |> ignore
            x.CreatedAt.Value.Should().Be(expectedCreatedAt.Value, "CreatedAt must be 1977-01-01") |> ignore
            x.Content.Value.Should().Contain(x.Error.Value.Value, "Content should contain error") |> ignore
            x.Content.Value.Should().Contain(x.Message.Value, "Content should contain message") |> ignore
        ) 


    member private me.MakeSut(?queueKeeper: IIndexingQueueKeeper, 
                              ?storageWriter: IStorageWriter): ITask =

        let queueKeeper = queueKeeper |? (new Mock<IIndexingQueueKeeper>(MockBehavior.Strict)).Object
        let storageWriter = storageWriter |? (new Mock<IStorageWriter>(MockBehavior.Strict)).Object

        new QueueProcessingTask(queueKeeper, storageWriter) :> ITask