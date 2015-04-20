namespace NBlast.Api.Tests

open System
open System.Runtime
open NUnit.Framework
open Moq
open FluentAssertions
open NBlast.Storage.Core.Index
open NBlast.Api.Controllers
open NBlast.Storage.Core
open System.Web.Http.Results

[<AllowNullLiteral>]
type TermSearcherControllerSpecs() =

    [<Test>]
    member me.``Term search should not work with wrong field``() =
        // Given
        let sut = me.MakeSut()

        // When
        let actual = sut.Search("bullshit", "go")

        // Then
        (actual).Should().BeOfType<BadRequestErrorMessageResult>("Bad request expected") |> ignore

    [<Test>]
    member me.``Search full of parameters must pass all of them to storage reader``() =
        // Given
        let query = "expression"
        let fromDate = DateTime.Now.AddDays(-2.0)
        let tillDate = DateTime.Now.AddDays(2.0)
        let term     = LogField.Sender

        let sq = {
            Expression = query 
            Take = Some 15 
            Skip = Some (0 * 15)
            Filter = FilterQuery.Between(fromDate, tillDate) |> Some
            Sort = {Reverse = true; Field = LogField.CreatedAt} |> Some
        }
        let result = {Hits = []; Total = 0; QueryDuration = 0L}
        let deps = me.MakeSutDependencies(term, sq, result)
        let sut = me.MakeSut(fst deps, snd deps)

        // When
        let actionResult = sut.Search(term.GetName(),
                                      query, 
                                      1, 
                                      "createdat", 
                                      new Nullable<_>(true),
                                      new Nullable<_>(fromDate),
                                      new Nullable<_>(tillDate)) :?> OkNegotiatedContentResult<LogDocumentHits>

        // Then
        Mock.Get(fst deps).VerifyAll() |> ignore
        actionResult.Content.Should().BeSameAs(result, "Same result is expected") |> ignore

    member private me.MakeSut(?reader: IStorageReader,
                              ?configReader: IConfigReader): TermSearcherController =
                               
        let reader = (new Mock<IStorageReader>(MockBehavior.Strict)).Object |> defaultArg reader
        let configReader = (new Mock<IConfigReader>(MockBehavior.Strict)).Object |> defaultArg configReader
        new TermSearcherController(reader, configReader)

    member private me.MakeSutDependencies(term, expression, result) = 

        let reader = new Mock<IStorageReader>(MockBehavior.Strict)
        let configReader = new Mock<IConfigReader>(MockBehavior.Strict)

        reader.Setup(fun x -> x.SearchByTerm(term, expression))
            .Returns(fun () -> result) |> ignore

        configReader.Setup(fun x -> x.ReadAsInt(It.IsAny<string>()))
            .Returns(fun () -> 15) |> ignore

        (reader.Object, configReader.Object)