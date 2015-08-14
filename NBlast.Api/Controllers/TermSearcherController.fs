namespace NBlast.Api.Controllers

open System
open System.Net
open System.Web.Http
open System.Net.Http
open System.Net.Http.Headers
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
    member me.Search (k, q) = me.Search(k, q, 1)

    [<HttpGet>]
    [<Route("search")>]
    member me.Search (k, q, p) =
        me.Search (k, q, p, null, new Nullable<_>(), new Nullable<_>(), new Nullable<_>())

    [<HttpGet>]
    [<Route("search-atom")>]
    member me.SearchAtom(k    : string,
                         q    : string,
                         p    : int,
                         sf   : string,
                         sr   : Nullable<Boolean>,
                         from : Nullable<DateTime>,
                         till : Nullable<DateTime>) =

        let response: HttpResponseMessage = me.Search(k, q, p, sf, sr ,from, till)
        response.Content.Headers.ContentType <- new MediaTypeHeaderValue("application/atom+xml");
        response

    [<HttpGet>]
    [<Route("search")>]
    member me.Search (k    : string,
                      q    : string,
                      p    : int,
                      sf   : string,
                      sr   : Nullable<Boolean>,
                      from : Nullable<DateTime>,
                      till : Nullable<DateTime>) =

        let logField = k |> LogField.ConvertFrom
        if logField.IsNone then
            me.Request.CreateResponse(HttpStatusCode.BadRequest, k |> sprintf "Invalid field name %s") 
        else
            let searchQuery = me.AssembleSearchQuery(q, p, itemsPerPage, sf, sr, from, till)
            let result = (storageReader.SearchByTerm (logField.Value, searchQuery))
            me.Request.CreateResponse(HttpStatusCode.OK, result)