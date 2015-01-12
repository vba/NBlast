namespace NBlast.Api.Tests

open System
open System.Runtime
open Xunit
open Moq
open FluentAssertions
open NBlast.Storage.Core.Index
open NBlast.Api.Controllers

type SearchControllerSpecs() = 

    [<Fact>]
    member me.``Count all must use storage reader in the right way``() =
        // Given
        let reader = new Mock<IStorageReader>(MockBehavior.Strict)
        let result = 0
        let sut = me.MakeSut(reader.Object)

        // When
        reader.Setup(fun x -> x.CountAll())
            .Returns(fun () -> result) |> ignore

        let result = sut.CountAll()

        // Then
        reader.VerifyAll() |> ignore
        result.Should().Be(0, "0 count is expected") |> ignore

    [<Fact>]
    member me.``Search action must work as expected``() =
        // Given
        let reader = new Mock<IStorageReader>(MockBehavior.Strict)
        let query = "some: value"
        let expression = SearchQuery.GetOnlyExpression query
        let result = {Hits = []; Total = 0; QueryDuration = 0L}
        let sut = me.MakeSut(reader.Object)

        // When
        reader.Setup(fun x -> x.SearchByField(expression))
            .Returns(fun () -> result) |> ignore

        let actionResult = sut.Search(query)

        // Then
        reader.VerifyAll() |> ignore
        actionResult.Should().BeSameAs(result, "Same result is expected") |> ignore

    [<Fact>]
    member me.``Get by id action must return 0 elements``() =
        // Given
        let reader = new Mock<IStorageReader>(MockBehavior.Strict)
        let id = Guid.NewGuid()
        let expression = {
            Expression = "id : " + id.ToString()
            Filter     = None
            Skip       = None
            Take       = Some 1
        }
        let result = {Hits = []; Total = 0; QueryDuration = 0L}
        let sut = me.MakeSut(reader.Object)

        // When
        reader.Setup(fun x -> x.SearchByField(expression))
            .Returns(fun () -> result) |> ignore

        let actionResult = sut.GetById(id)
        
        // Then
        reader.VerifyAll() |> ignore
        actionResult.Should().BeSameAs(result, "Same result is expected") |> ignore

    member private me.MakeSut(?reader: IStorageReader): SearcherController = 
        let reader = (new Mock<IStorageReader>(MockBehavior.Strict)).Object |> defaultArg reader
        new SearcherController(reader)