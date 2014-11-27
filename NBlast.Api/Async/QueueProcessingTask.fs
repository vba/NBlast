namespace NBlast.Api.Async

open NBlast.Api.Models
open FluentScheduler
open NBlast.Api.Models
open NBlast.Storage
open NBlast.Storage.Core.Index


type QueueProcessingTask(queueKeeper: IIndexingQueueKeeper, 
                         storageWriter: IStorageWriter) =
    
    static let logger = NLog.LogManager.GetCurrentClassLogger()

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
            logger.Debug("Scheduled task executed, queue contains {0} element(s)", queueKeeper.Count())
            //queueKeeper.ConsumeMany(Some 20) |> me.ProcessModels |> ignore  

