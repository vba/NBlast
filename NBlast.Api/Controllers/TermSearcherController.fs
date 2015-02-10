namespace NBlast.Api.Controllers

open System
open System.Web.Http
open System.Net.Http.Formatting
open NBlast.Storage.Core.Index
open NBlast.Storage.Core
open NBlast.Storage.Core.Extensions
open System.Web.Http.Cors


[<RoutePrefix("api/term-searcher")>]
[<EnableCors("*", "*", "GET")>]
type TermSearcherController(storageReader: IStorageReader,
                            configReader: IConfigReader) = 
    inherit ApiController()

    static let logger = NLog.LogManager.GetCurrentClassLogger()
    let itemsPerPage  = lazy("NBlast.search.hits_per_page" |> configReader.ReadAsInt)

    [<HttpGet>]
    [<Route("search")>]
    member me.TermSearch (k, q) = me.TermSearch(k, q, 1)

    [<HttpGet>]
    [<Route("search")>]
    member me.TermSearch (k, q, p) =
        me.TermSearch (k, q, p, null, new Nullable<_>(), new Nullable<_>(), new Nullable<_>())

    [<HttpGet>]
    [<Route("search")>]
    member me.TermSearch (k    : string,
                          q    : string,
                          p    : int,
                          sf   : string,
                          sr   : Nullable<Boolean>,
                          from : Nullable<DateTime>,
                          till : Nullable<DateTime>) =

        let logField = k |> LogField.ConvertFrom
        if logField.IsNone then
            k |> sprintf "Invalid field name %s" |> me.BadRequest :> IHttpActionResult
        else
            let searchQuery = me.AssembleSearchQuery(q, p, itemsPerPage, sf, sr, from, till)
            searchQuery |> sprintf "Term search with term key %s and %A" k |> logger.Debug
            let result = (searchQuery |> storageReader.SearchByTerm logField.Value) 
            me.Ok(result) :> IHttpActionResult