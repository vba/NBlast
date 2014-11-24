namespace NBlast.Api.Tests.Async

open NBlast.Api.Async
open NBlast.Api.Models
open System
open System.Runtime
open Xunit
open FluentAssertions
open Ploeh.AutoFixture
open Ploeh.AutoFixture.Kernel
open System.Threading.Tasks
open FSharp.Collections.ParallelSeq

type IndexingQueueKeeperTest() =

    [<Fact>]
    member me.``Keeper must enqueue models in the ordinary context``() =
        // Given
        let sut = me.MakeSut()
        let models = me.``Gimme N log models``()
        
        // When
        models |> Seq.iter (fun x -> sut.Enqueue(x).Wait())
         
        // Then
        sut.Count().Should().Be(10, "Queue count must be 10")

    [<Fact>]
    member me.``Keeper must enqueue models in the parallel context``() =
        // Given
        let sut = me.MakeSut()
        let models = me.``Gimme N log models``()
        
        // When
        models |> PSeq.iter (fun x -> sut.Enqueue(x) |> ignore)
        System.Threading.Thread.Sleep(500)

        // Then
        sut.Count().Should().Be(10, "Queue count must be 10")


    
    member private me.``Gimme N log models`` ?x =
        (new Fixture() :> IFixture).CreateMany<LogModel>(defaultArg x 10) |> Seq.toList

    member private me.MakeSut(): IQueueKeeper<LogModel> = 
        new IndexingQueueKeeper() :> IQueueKeeper<LogModel>