namespace NBlast.Api.Async

open NBlast.Api.Models
open FluentScheduler
open NBlast.Storage
open NBlast.Storage.Core.Index
open NBlast.Storage.Core.Extensions


type QueueProcessingTask(queueKeeper: IIndexingQueueKeeper, 
                         storageWriter: IStorageWriter,
                         ?indexingLimit: int) =
    
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
            queueKeeper.ConsumeMany(Some (indexingLimit |? 400)) |> me.ProcessModels |> ignore  

