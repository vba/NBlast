namespace NBlast.Api

open NBlast.Storage
open NBlast.Storage.Core.Index
open System.Web.Http
open System.Net.Http.Formatting

type IndexerController(storageWriter: IStorageWriter) =
    inherit ApiController()


    [<HttpPost>]
    //[<Route("index")>]
    member me.Index sender message logger level =
        let logDocument = new LogDocument(sender, message, logger, level) :> IStorageDocument
        logDocument |> storageWriter.InsertOne
        1

//module IndexerController

