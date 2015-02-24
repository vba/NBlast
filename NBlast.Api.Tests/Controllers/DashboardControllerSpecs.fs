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
    member me.``Levels per month should use storage reader and call all expected search by term``() =
        // Given
        let from = new DateTime(DateTime.Today.AddMonths(0).Year, DateTime.Today.AddMonths(0).Month, 1)
        let reader = new Mock<IStorageReader>(MockBehavior.Strict)
        let sut = me.MakeSut(reader.Object)
        let expectedResult = {Total = 3; Hits = []; QueryDuration = 0L}
        let query = fun x -> 
            { SearchQuery.GetOnlyExpression(x) 
               with Take = Some(1)
                    Filter = FilterQuery.Between(from, from.AddMonths(1).AddDays(-1.0).AddHours(23.0).AddMinutes(59.0).AddSeconds(59.0)) |> Some }

        reader.Setup(fun x -> x.SearchByTerm(LogField.Level, query("trace"))).Returns(expectedResult) |> ignore
        reader.Setup(fun x -> x.SearchByTerm(LogField.Level, query("info"))).Returns(expectedResult) |> ignore
        reader.Setup(fun x -> x.SearchByTerm(LogField.Level, query("debug"))).Returns(expectedResult) |> ignore
        reader.Setup(fun x -> x.SearchByTerm(LogField.Level, query("warn"))).Returns(expectedResult) |> ignore
        reader.Setup(fun x -> x.SearchByTerm(LogField.Level, query("error"))).Returns(expectedResult) |> ignore
        reader.Setup(fun x -> x.SearchByTerm(LogField.Level, query("fatal"))).Returns(expectedResult) |> ignore

        // Then
        let actualResult =  sut.``Levels per month``(0)

        reader.VerifyAll() |> ignore
        actualResult.HasValues.Should().BeTrue("It must have values") |> ignore
        actualResult.Count.Should().Be(6, "6 keys are expected") |> ignore

    [<Fact>]
    member me.``Levels per week should use storage reader and call all expected search by term``() =
        // Given
        let from = DateTime.Today.AddDays(-1.0 * float(DateTime.Today.DayOfWeek)).AddDays(-3.0 * 7.0)
        let reader = new Mock<IStorageReader>(MockBehavior.Strict)
        let sut = me.MakeSut(reader.Object)
        let expectedResult = {Total = 3; Hits = []; QueryDuration = 0L}
        let query = fun x -> 
            { SearchQuery.GetOnlyExpression(x) 
               with Take = Some(1)
                    Filter = FilterQuery.Between(from, from.AddDays(6.0).AddHours(23.0).AddMinutes(59.0).AddSeconds(59.0)) |> Some }

        reader.Setup(fun x -> x.SearchByTerm(LogField.Level, query("trace"))).Returns(expectedResult) |> ignore
        reader.Setup(fun x -> x.SearchByTerm(LogField.Level, query("info"))).Returns(expectedResult) |> ignore
        reader.Setup(fun x -> x.SearchByTerm(LogField.Level, query("debug"))).Returns(expectedResult) |> ignore
        reader.Setup(fun x -> x.SearchByTerm(LogField.Level, query("warn"))).Returns(expectedResult) |> ignore
        reader.Setup(fun x -> x.SearchByTerm(LogField.Level, query("error"))).Returns(expectedResult) |> ignore
        reader.Setup(fun x -> x.SearchByTerm(LogField.Level, query("fatal"))).Returns(expectedResult) |> ignore

        // Then
        let actualResult =  sut.``Levels per week``(-3.0)

        reader.VerifyAll() |> ignore
        actualResult.HasValues.Should().BeTrue("It must have values") |> ignore
        actualResult.Count.Should().Be(6, "6 keys are expected") |> ignore

    [<Fact>]
    member me.``Levels per day should use storage reader and call all expected search by term``() =
        // Given
        let today = DateTime.Today.Date.AddDays(-2.0)
        let reader = new Mock<IStorageReader>(MockBehavior.Strict)
        let sut = me.MakeSut(reader.Object)
        let expectedResult = {Total = 3; Hits = []; QueryDuration = 0L}
        let query = fun x -> 
            { SearchQuery.GetOnlyExpression(x) 
               with Take = Some(1)
                    Filter = FilterQuery.Between(today, today.AddHours(23.0).AddMinutes(59.0).AddSeconds(59.0)) |> Some }

        reader.Setup(fun x -> x.SearchByTerm(LogField.Level, query("trace"))).Returns(expectedResult) |> ignore
        reader.Setup(fun x -> x.SearchByTerm(LogField.Level, query("info"))).Returns(expectedResult) |> ignore
        reader.Setup(fun x -> x.SearchByTerm(LogField.Level, query("debug"))).Returns(expectedResult) |> ignore
        reader.Setup(fun x -> x.SearchByTerm(LogField.Level, query("warn"))).Returns(expectedResult) |> ignore
        reader.Setup(fun x -> x.SearchByTerm(LogField.Level, query("error"))).Returns(expectedResult) |> ignore
        reader.Setup(fun x -> x.SearchByTerm(LogField.Level, query("fatal"))).Returns(expectedResult) |> ignore

        // Then
        let actualResult =  sut.``Levels per day``(-2.0)

        reader.VerifyAll() |> ignore
        actualResult.HasValues.Should().BeTrue("It must have values") |> ignore
        actualResult.Count.Should().Be(6, "6 keys are expected") |> ignore
        

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