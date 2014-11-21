namespace NBlast.Api

open NBlast.Storage
open NBlast.Storage.Core.Index
open System.Web.Http
open System.Net.Http.Formatting
open System

type LogDto () =
    let mutable error: string option = None
    let mutable createdAt: DateTime option = None

    member val Sender: string = null with get, set
    member val Message: string = null with get, set
    member val Logger: string = null with get, set
    member val Level: string = null with get, set
    
    member me.Error 
        with set (value: string) = 
          error <- if(String.IsNullOrEmpty(value)) then None else Some value

    member me.CreatedAt 
        with set (value: Nullable<DateTime>) = 
          createdAt <- if(value.HasValue) then Some value.Value else None

    member me.ErrorOp with get() = error
    member me.CreatedAtOp with get() = createdAt

type IndexerController(storageWriter: IStorageWriter) =
    inherit ApiController()


    [<HttpPost>]
    //[<Route("index")>]
    member me.Index (logDto: LogDto) =
        let logDocument = new LogDocument(logDto.Sender, 
                                          logDto.Message, 
                                          logDto.Logger, 
                                          logDto.Level,
                                          logDto.ErrorOp,
                                          logDto.CreatedAtOp) :> IStorageDocument

        logDocument |> storageWriter.InsertOne
        me.Created(me.Request.RequestUri, logDocument)


