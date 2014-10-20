namespace NBlast.Api.Tests

open System
open System.Runtime
open Xunit
open FluentAssertions

type HomeControllerSpecs() = 

    [<Fact>]
    member this.``Some feature``() =
        (true).Should().Be(true, "True must be true") |> ignore
