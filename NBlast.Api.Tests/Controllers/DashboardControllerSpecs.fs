namespace NBlast.Api.Tests

open System
open System.Runtime
open Xunit
open Moq
open FluentAssertions
open NBlast.Storage.Core.Index
open NBlast.Api.Controllers
open NBlast.Storage.Core
open System.Web.Http.Results

[<AllowNullLiteral>]
type DashboardControllerSpecs() = 

    [<Fact>]
    member me.``Group by field should use storage reader and respect facet limit``() =
        // Given
        let limit = 1
        let reader = new Mock<IStorageReader>(MockBehavior.Strict)
        let sut = me.MakeSut(reader.Object)
        let actualFacets = { QueryDuration = 0L
                             Total = 3
                             Facets = [{ Name = "Sender1"; Count = 2L }
                                       { Name = "Sender2"; Count = 2L }
                                       { Name = "Sender3"; Count = 2L }] }

        reader
            .Setup(fun x -> x.GroupWith(LogField.Sender))
            .Returns(fun () -> actualFacets) |> ignore

        // When
        let result = sut.GroupByField("sender", limit)

        // Then
        result.Should()
            .BeOfType<OkNegotiatedContentResult<SimpleFacets>>("Ok request expected") |> ignore
        let content = (result :?> OkNegotiatedContentResult<SimpleFacets>).Content
        content.Facets.Should().HaveCount(limit, "Limit must be respected") |> ignore
        content.Total.Should().Be(actualFacets.Total, "Total must remain the same") |> ignore

    [<Fact>]
    member me.``Group by field should not work with wrong field``() =
        // Given
        let sut = me.MakeSut()

        // When
        let actual = sut.GroupByField("bullshit")

        // Then
        (actual).Should().BeOfType<BadRequestErrorMessageResult>("Bad request expected")

    member private me.MakeSut(?reader: IStorageReader,
                              ?configReader: IConfigReader): DashboardController =
                               
        let reader = (new Mock<IStorageReader>(MockBehavior.Strict)).Object |> defaultArg reader
        let configReader = (new Mock<IConfigReader>(MockBehavior.Strict)).Object |> defaultArg configReader
        new DashboardController(reader, configReader)