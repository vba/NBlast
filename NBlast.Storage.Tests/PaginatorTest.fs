namespace NBlast.Storage.Tests

open System
open System.Runtime
open Xunit
open FluentAssertions
open NBlast.Storage
open NBlast.Storage.Core

type PaginatorTest() = 

    member private this.MakeSut() = new Paginator() :> IPaginator

    [<Fact>]
    member this.``Get pages sections returns truncated section``() = 
        // Given
        let sut = this.MakeSut()

        //When
        let result = sut.GetFollowingSection 15 10 20 |> Seq.toList

        //Then
        (result |> List.head).Should().Be(16, "Second page section must start with 16") |> ignore
        (result |> List.rev  |> List.head).Should().Be(20, "Second page section must end with 20") |> ignore

    [<Fact>]
    member this.``Get pages sections returns truncated section with only one element``() = 
        // Given
        let sut = this.MakeSut()

        //When
        let result = sut.GetFollowingSection 19 10 20 |> Seq.toList

        //Then
        (result.Length).Should().Be(1, "Result must containt only one item") |> ignore
        (result |> List.head).Should().Be(20, "Second page section must start with 20") |> ignore

    [<Fact>]
    member this.``Get pages sections returns empty sequence when skip equals limit``() = 
        // Given
        let sut = this.MakeSut()

        //When
        let result = sut.GetFollowingSection 20 10 20 |> Seq.toList

        //Then
        (result.Length).Should().Be(0, "Result must be empty") |> ignore

    [<Fact>]
    member this.``Get pages sections returns empty sequence when skip bypass limit``() = 
        // Given
        let sut = this.MakeSut()

        //When
        let result = sut.GetFollowingSection 25 10 20 |> Seq.toList

        //Then
        (result.Length).Should().Be(0, "Result must be empty") |> ignore

    [<Fact>]
    member this.``Get pages sections returns complete section``() = 
        // Given
        let sut = this.MakeSut()

        //When
        let result = sut.GetFollowingSection 0 5 20 |> Seq.toList

        //Then
        (result.Length).Should().Be(5, "Result lenght must be equal to 5") |> ignore
        (result |> List.head).Should().Be(1, "First page section must start with 1") |> ignore
        (result |> List.rev |> List.head).Should().Be(5, "Second page section must start with 6") |> ignore

    [<Fact>]
    member this.``Get pages sections returns complete section respecting total limit``() = 
        // Given
        let sut = this.MakeSut()

        //When
        let result = sut.GetFollowingSection 15 5 20 |> Seq.toList

        //Then
        (result.Length).Should().Be(5, "Result lenght must be equal to 5") |> ignore
        (result |> List.head).Should().Be(16, "First page section must start with 16") |> ignore
        (result |> List.rev |> List.head).Should().Be(20, "Second page section must start with 20") |> ignore

    [<Fact>]
    member this.``Get pages sections returns truncated section respecting total limit``() = 
        // Given
        let sut = this.MakeSut()

        //When
        let result = sut.GetFollowingSection 16 5 20 |> Seq.toList

        //Then
        (result.Length).Should().Be(4, "Result lenght must be equal to 4") |> ignore
        (result |> List.head).Should().Be(17, "First page section must start with 17") |> ignore
        (result |> List.rev |> List.head).Should().Be(20, "Second page section must start with 20") |> ignore