namespace NBlast.Api.Controllers

open NBlast.Api.Models
open NBlast.Storage
open NBlast.Storage.Core.Index
open System.Web.Http
open System.Net.Http.Formatting

type IndexerController(storageWriter: IStorageWriter) =
    inherit ApiController()


    [<HttpPost>]
    //[<Route("index")>]
    member me.Index (model: LogModel) =
        if (me.ModelState.IsValid) then
            let logDocument = new LogDocument(model.Sender, 
                                              model.Message, 
                                              model.Logger, 
                                              model.Level,
                                              model.ErrorOp,
                                              model.CreatedAtOp) :> IStorageDocument

            logDocument |> storageWriter.InsertOne
            me.Created(me.Request.RequestUri, logDocument) :> IHttpActionResult
        else
            me.BadRequest(me.ModelState) :> IHttpActionResult


