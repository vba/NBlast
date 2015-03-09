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
        let sut = me.MakeSut()

        // When
        let actual = sut.Top(10)
        
        // Then
        actual.Should().NotBeNull("Expected resutl should never be null")

    member private me.MakeSut(?queueKeeper: IIndexingQueueKeeper): QueueController =
                               
        let queueKeeper = (new Mock<IIndexingQueueKeeper>(MockBehavior.Strict)).Object |> defaultArg queueKeeper
        new QueueController(queueKeeper)