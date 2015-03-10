namespace NBlast.Api.Tests

open System
open System.Runtime
open Xunit
open Moq
open FluentAssertions
open NBlast.Storage.Core.Index
open NBlast.Api.Controllers
open NBlast.Api.Async
open NBlast.Storage.Core
open System.Web.Http.Results

[<AllowNullLiteral>]
type QueueControllerSpecs() =

    [<Fact>]
    member me.``Queue requesting should always return a result``() =
        // Given
        let queueKeeper = new Mock<IIndexingQueueKeeper>(MockBehavior.Strict)
        let sut = me.MakeSut(queueKeeper.Object)

        queueKeeper.Setup(fun x -> x.Count()).Returns(0) |> ignore
        queueKeeper.Setup(fun x -> x.PeekTop(10)).Returns([]) |> ignore

        // When
        let actual = sut.Top(10)
        
        // Then
        queueKeeper.VerifyAll() |> ignore
        actual.Should().NotBeNull("Expected resutl should never be null") |> ignore
        actual.Logs.ShouldBeEquivalentTo([], "Empty logs expected") |> ignore
        actual.Total.ShouldBeEquivalentTo(0, "Empty total expected") |> ignore

    member private me.MakeSut(?queueKeeper: IIndexingQueueKeeper): QueueController =
                               
        let queueKeeper = (new Mock<IIndexingQueueKeeper>(MockBehavior.Strict)).Object |> defaultArg queueKeeper
        new QueueController(queueKeeper)