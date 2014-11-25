namespace NBlast.Api.Async

open NBlast.Api.Models
open FluentScheduler
open NBlast.Api.Models
open NBlast.Storage
open NBlast.Storage.Core.Index


type QueueProcessingTask(queueKeeper: IQueueKeeper<LogModel>, 
                         storageWriter: IStorageWriter) =

    member private me.ProcessModels (models: seq<LogModel>) =
        models |> Seq.toList |> List.iter (fun model ->
            let logDocument = new LogDocument(model.Sender, 
                                              model.Message, 
                                              model.Logger, 
                                              model.Level,
                                              model.ErrorOp,
                                              model.CreatedAtOp) :> IStorageDocument

            logDocument |> storageWriter.InsertOne

        )
    interface ITask with 
        member me.Execute() =
            queueKeeper.ConsumeMany(Some 10) |> me.ProcessModels |> ignore  

