namespace NBlast.Api.Tests

open System
open System.Runtime
open Xunit
open Moq
open FluentAssertions
open NBlast.Storage.Core.Index
open NBlast.Api.Controllers

type IndexerControllerSpecs() = 

    [<Fact>]
    member me.``Check few facts``() =
        (true).Should().Be(true, "True should be true, isn't")