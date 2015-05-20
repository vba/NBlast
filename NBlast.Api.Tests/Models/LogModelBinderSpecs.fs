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
open System

type Capture = {
    mutable model: LogModel
}

[<Ignore>]
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

    [<TestCase("message=some text&logger=logger&sender=sender1&level=fatal&error=error", "some text", "logger", "sender1", "fatal", "error", null)>]
    [<TestCase("message=some+text+with+%26+and+%25&logger=logger&sender=sender1&level=fatal&error=error", "some text with & and %", "logger", "sender1", "fatal", "error", null)>]
    [<TestCase("{\"message\":\"&done\"}", "&done", null, null, null, null, null)>]
    [<TestCase("&", null, null, null, null, null, null)>]
    [<TestCase("", null, null, null, null, null, null)>]
    member me.``Model binding must work as expected with supplied raw body``(raw, message, logger, sender, level, error, creationDate: Nullable<DateTime>) =
        // Given
        let queue  = new Queue<LogModel>()
        let sut    = me.MakeSut()
        

        // When
        (fun x -> queue.Enqueue(x); true) |> sut.BindFromStringValue(raw) |> ignore
         
        // Then
        queue.Count.Should().Be(1, "Queue must capture one element") |> ignore
        queue.Peek().Should().NotBeNull("Queued element cannot be a null") |> ignore
        queue.Peek().Message.Should().Be(message, "Mesage should be equivalent") |> ignore
        queue.Peek().Logger.Should().Be(logger, "Logger should be equivalent") |> ignore
        queue.Peek().Sender.Should().Be(sender, "Sender should be equivalent") |> ignore
        queue.Peek().Level.Should().Be(level, "Level should be equivalent") |> ignore
        queue.Peek().Error.Should().Be(error, "Error should be equivalent") |> ignore
        queue.Peek().CreatedAt.Should().Be(creationDate, "Error should be equivalent") |> ignore


    member private me.MakeSut(): LogModelBinder = new LogModelBinder()