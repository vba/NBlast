namespace NBlast.Api.Controllers

open NBlast.Api.Models
open NBlast.Api.Async
open NBlast.Storage
open NBlast.Storage.Core.Index
open System.Web.Http
open System.Net.Http.Formatting

type IndexerController(queueKeeper: IIndexingQueueKeeper) =
    inherit ApiController()

    [<HttpPost>]
    //[<Route("index")>]
    member me.Index (model: LogModel) =
        if (me.ModelState.IsValid) then
            queueKeeper.Enqueue(model)
            me.Ok(model) :> IHttpActionResult
        else
            me.BadRequest(me.ModelState) :> IHttpActionResult


