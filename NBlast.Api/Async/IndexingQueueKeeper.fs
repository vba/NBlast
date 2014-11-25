namespace NBlast.Api.Async

open NBlast.Api.Models
open NBlast.Storage
open NBlast.Storage.Core.Index
open NBlast.Storage.Core.Extensions
open System.Collections.Concurrent
open System.Threading.Tasks

type IQueueKeeper<'a> = interface
    abstract member Enqueue : 'a -> Task<unit>
    abstract member Consume : unit -> 'a option
    abstract member ConsumeMany : int option -> seq<'a>
    abstract member Count : unit -> int 
end

type IndexingQueueKeeper() =
    static let logger = NLog.LogManager.GetCurrentClassLogger()
    let queue = new ConcurrentQueue<LogModel>()

    interface IQueueKeeper<LogModel> with
        member me.Count() = queue |> Seq.length 
        member me.Enqueue model = 
            async {
                model |> sprintf "Enqueuing model %A" |> logger.Debug
                queue.Enqueue(model)
            } 
            |> Async.StartAsTask

        member me.Consume() = 
            match queue.TryDequeue() with
            | (false, _) -> None
            | (true, model) ->
                model |> sprintf "Dequeuing model %A" |> logger.Debug 
                Some model

        member me.ConsumeMany ?amount =
            let consume = (me :> IQueueKeeper<LogModel>).Consume
            [ 0 .. (amount |? 10) - 1 ] |> Seq.map (fun x -> consume()) 
                |> Seq.filter Option.isSome 
                |> Seq.map Option.get 
            