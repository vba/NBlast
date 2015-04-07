namespace NBlast.Api.Tests

open System
open System.Runtime
open NUnit.Framework
open Moq
open FluentAssertions
open NBlast.Storage.Core.Index
open NBlast.Api.Controllers
open NBlast.Storage.Core

[<AllowNullLiteral>]
type SearcherControllerSpecs() = 

    [<Test>]
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

    [<Test>]
    member me.``Search full of parameters must pass all of them to storage reader``() =
        // Given
        let query = "expression"
        let fromDate = DateTime.Now.AddDays(-2.0)
        let tillDate = DateTime.Now.AddDays(2.0)

        let sq = {
            Expression = query 
            Take = Some 15 
            Skip = Some (0 * 15)
            Filter = FilterQuery.Between(fromDate, tillDate) |> Some
            Sort = {Reverse = true; Field = LogField.CreatedAt} |> Some
        }
        let result = {Hits = []; Total = 0; QueryDuration = 0L}
        let deps = me.MakeSutDependencies(sq, result)
        let sut = me.MakeSut(fst deps, snd deps)

        // When
        let actionResult = sut.Search(query, 
                                      1, 
                                      "createdat", 
                                      new Nullable<_>(true),
                                      new Nullable<_>(fromDate),
                                      new Nullable<_>(tillDate))

        // Then
        Mock.Get(fst deps).VerifyAll() |> ignore
        actionResult.Should().BeSameAs(result, "Same result is expected") |> ignore

    [<Test>]
    member me.``Search with unknown sort field must ignore sort at all``() =
        // Given
        let query = "expression"
        let fromDate = DateTime.Now.AddDays(-2.0)

        let sq = {
            Expression = query 
            Take = Some 15 
            Skip = Some (0 * 15)
            Filter = FilterQuery.After(fromDate) |> Some
            Sort = None
        }
        let result = {Hits = []; Total = 0; QueryDuration = 0L}
        let deps = me.MakeSutDependencies(sq, result)
        let sut = me.MakeSut(fst deps, snd deps)

        // When
        let actionResult = sut.Search(query, 
                                      1, 
                                      "Unknown", 
                                      new Nullable<_>(true),
                                      new Nullable<_>(fromDate),
                                      new Nullable<_>())

        // Then
        Mock.Get(fst deps).VerifyAll() |> ignore
        actionResult.Should().BeSameAs(result, "Same result is expected") |> ignore

    [<Test>]
    member me.``Search without reverse parameter must ignore it``() =
        // Given
        let query = "expression"
        let fromDate = DateTime.Now.AddDays(-2.0)

        let sq = {
            Expression = query 
            Take = Some 15 
            Skip = Some (0 * 15)
            Filter = FilterQuery.After(fromDate) |> Some
            Sort = {Reverse = false; Field = LogField.CreatedAt} |> Some
        }
        let result = {Hits = []; Total = 0; QueryDuration = 0L}
        let deps = me.MakeSutDependencies(sq, result)
        let sut = me.MakeSut(fst deps, snd deps)

        // When
        let actionResult = sut.Search(query, 
                                      1, 
                                      "createdat", 
                                      new Nullable<_>(),
                                      new Nullable<_>(fromDate),
                                      new Nullable<_>())

        // Then
        Mock.Get(fst deps).VerifyAll() |> ignore
        actionResult.Should().BeSameAs(result, "Same result is expected") |> ignore

    [<Test>]
    member me.``Search with after parameter must pass it to search``() =
        // Given
        let query = "expression"
        let fromDate = DateTime.Now.AddDays(-2.0)

        let sq = {
            Expression = query 
            Take = Some 15 
            Skip = Some (0 * 15)
            Filter = FilterQuery.After(fromDate) |> Some
            Sort = {Reverse = true; Field = LogField.CreatedAt} |> Some
        }
        let result = {Hits = []; Total = 0; QueryDuration = 0L}
        let deps = me.MakeSutDependencies(sq, result)
        let sut = me.MakeSut(fst deps, snd deps)

        // When
        let actionResult = sut.Search(query, 
                                      1, 
                                      "createdat", 
                                      new Nullable<_>(true),
                                      new Nullable<_>(fromDate),
                                      new Nullable<_>())

        // Then
        Mock.Get(fst deps).VerifyAll() |> ignore
        actionResult.Should().BeSameAs(result, "Same result is expected") |> ignore

    [<Test>]
    member me.``Search with before parameter must pass it to search``() =
        // Given
        let query = "expression"
        let tillDate = DateTime.Now.AddDays(2.0)

        let sq = {
            Expression = query 
            Take = Some 15 
            Skip = Some (0 * 15)
            Filter = FilterQuery.Before(tillDate) |> Some
            Sort = {Reverse = true; Field = LogField.CreatedAt} |> Some
        }
        let result = {Hits = []; Total = 0; QueryDuration = 0L}
        let deps = me.MakeSutDependencies(sq, result)
        let sut = me.MakeSut(fst deps, snd deps)

        // When
        let actionResult = sut.Search(query, 
                                      1, 
                                      "createdat", 
                                      new Nullable<_>(true),
                                      new Nullable<_>(),
                                      new Nullable<_>(tillDate))

        // Then
        Mock.Get(fst deps).VerifyAll() |> ignore
        actionResult.Should().BeSameAs(result, "Same result is expected") |> ignore

    [<Test>]
    member me.``Search without date parameters must pass all to search``() =
        // Given
        let query = "expression"

        let sq = {
            Expression = query 
            Take = Some 15 
            Skip = Some (0 * 15)
            Filter = None
            Sort = {Reverse = true; Field = LogField.CreatedAt} |> Some
        }
        let result = {Hits = []; Total = 0; QueryDuration = 0L}
        let deps = me.MakeSutDependencies(sq, result)
        let sut = me.MakeSut(fst deps, snd deps)

        // When
        let actionResult = sut.Search(query, 
                                      1, 
                                      "createdat", 
                                      new Nullable<_>(true),
                                      new Nullable<_>(),
                                      new Nullable<_>())

        // Then
        Mock.Get(fst deps).VerifyAll() |> ignore
        actionResult.Should().BeSameAs(result, "Same result is expected") |> ignore

    [<Test>]
    member me.``Search without date and with not reversed sort parameters must pass all to search``() =
        // Given
        let query = "expression"

        let sq = {
            Expression = query 
            Take = Some 15 
            Skip = Some (0 * 15)
            Filter = None
            Sort = {Reverse = false; Field = LogField.Sender} |> Some
        }
        let result = {Hits = []; Total = 0; QueryDuration = 0L}
        let deps = me.MakeSutDependencies(sq, result)
        let sut = me.MakeSut(fst deps, snd deps)

        // When
        let actionResult = sut.Search(query, 
                                      1, 
                                      "sender", 
                                      new Nullable<_>(false),
                                      new Nullable<_>(),
                                      new Nullable<_>())

        // Then
        Mock.Get(fst deps).VerifyAll() |> ignore
        actionResult.Should().BeSameAs(result, "Same result is expected") |> ignore

    [<Test>]
    member me.``Search action must work as expected``() =
        // Given
        let reader = new Mock<IStorageReader>(MockBehavior.Strict)
        let configReader = new Mock<IConfigReader>(MockBehavior.Strict)
        let query = "some: value"
        let expression = {
            SearchQuery.GetOnlyExpression query 
            with Take = Some 15 
                 Skip = Some (0 * 15)
        }
        let result = {Hits = []; Total = 0; QueryDuration = 0L}
        let sut = me.MakeSut(reader.Object, configReader.Object)

        // When
        reader.Setup(fun x -> x.SearchByField(expression))
            .Returns(fun () -> result) |> ignore

        configReader.Setup(fun x -> x.ReadAsInt(It.IsAny<string>()))
            .Returns(fun () -> 15) |> ignore

        let actionResult = sut.Search(query)

        // Then
        reader.VerifyAll() |> ignore
        actionResult.Should().BeSameAs(result, "Same result is expected") |> ignore

    [<Test>]
    member me.``Get by id action must return 0 elements``() =
        // Given
        let reader = new Mock<IStorageReader>(MockBehavior.Strict)
        let id = Guid.NewGuid()
        let expression = {
            Expression = "id : " + id.ToString()
            Filter     = None
            Skip       = None
            Sort       = None
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


    member private me.MakeSutDependencies(expression, result) = 

        let reader = new Mock<IStorageReader>(MockBehavior.Strict)
        let configReader = new Mock<IConfigReader>(MockBehavior.Strict)

        reader.Setup(fun x -> x.SearchByField(expression))
            .Returns(fun () -> result) |> ignore

        configReader.Setup(fun x -> x.ReadAsInt(It.IsAny<string>()))
            .Returns(fun () -> 15) |> ignore

        (reader.Object, configReader.Object)

    member private me.MakeSut(?reader: IStorageReader,
                              ?configReader: IConfigReader): SearcherController =
                               
        let reader = (new Mock<IStorageReader>(MockBehavior.Strict)).Object |> defaultArg reader
        let configReader = (new Mock<IConfigReader>(MockBehavior.Strict)).Object |> defaultArg configReader
        new SearcherController(reader, configReader)