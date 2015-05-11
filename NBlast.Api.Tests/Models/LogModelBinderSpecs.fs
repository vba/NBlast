namespace NBlast.Api.Tests.Models

open NBlast.Api.Models
open NUnit.Framework
open FluentAssertions
open Ploeh.AutoFixture
open Ploeh.AutoFixture.Kernel
open System.Web.Http.ModelBinding
open System.Web.Http.Controllers
open System.Web.Http.Metadata
open System.Collections.Generic

type Capture = {
    mutable model: LogModel
}

type LogModelBinderSpecs() =

    [<Test>]
    member me.``Model binding must work as expected with an empty data stream``() =
        // Given
        let queue  = new Queue<LogModel>()
        let sut    = me.MakeSut()

        // When
        (fun x -> queue.Enqueue(x); true) |> sut.BindFromStringValue("") |> ignore
         
        // Then
        queue.Count.Should().Be(1, "Queue must capture one element") |> ignore
        queue.Peek().Should().NotBeNull("Queued element cannot be a null") |> ignore


    [<Test>]
    member me.``Model binding must work as expected with an invalid data stream``() =
        // Given
        let queue  = new Queue<LogModel>()
        let sut    = me.MakeSut()

        // When
        (fun x -> queue.Enqueue(x); true) |> sut.BindFromStringValue("cartman was here") |> ignore
         
        // Then
        queue.Count.Should().Be(1, "Queue must capture one element") |> ignore
        queue.Peek().Should().NotBeNull("Queued element cannot be a null") |> ignore

    [<Test>]
    member me.``Model binding must work as expected with an invalid data stream format``() =
        // Given
        let queue  = new Queue<LogModel>()
        let sut    = me.MakeSut()

        // When
        (fun x -> queue.Enqueue(x); true) |> sut.BindFromStringValue("0") |> ignore
         
        // Then
        queue.Count.Should().Be(1, "Queue must capture one element") |> ignore
        queue.Peek().Should().NotBeNull("Queued element cannot be a null") |> ignore

    [<Test>]
    member me.``Model binding must work as expected with a json nulls``() =
        // Given
        let queue  = new Queue<LogModel>()
        let sut    = me.MakeSut()

        // When
        (fun x -> queue.Enqueue(x); true) |> sut.BindFromStringValue("null") |> ignore
         
        // Then
        queue.Count.Should().Be(1, "Queue must capture one element") |> ignore
        queue.Peek().Should().NotBeNull("Queued element cannot be a null") |> ignore

    member private me.MakeSut(): LogModelBinder = new LogModelBinder()