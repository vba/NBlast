namespace NBlast.Api.Controllers

open System
open System.Web.Http
open System.Net.Http.Formatting
open NBlast.Storage.Core.Index
open NBlast.Storage.Core
open NBlast.Storage.Core.Extensions
open System.Web.Http.Cors


[<RoutePrefix("api/searcher")>]
[<EnableCors("*", "*", "GET")>]
type SearcherController(storageReader: IStorageReader,
                        configReader: IConfigReader) = 
    inherit ApiController()

    static let logger = NLog.LogManager.GetCurrentClassLogger()
    let itemsPerPage  = lazy("NBlast.search.hits_per_page" |> configReader.ReadAsInt)

    [<HttpGet>]
    [<Route("search")>]
    [<Route("search/{q}")>]
    member me.Search q = me.Search(q, 1)

    [<HttpGet>]
    [<Route("search")>]
    [<Route("search/{p}/{q}")>]
    member me.Search (q, p) =
        me.Search (q, p, null, new Nullable<_>(), new Nullable<_>(), new Nullable<_>())

    [<HttpGet>]
    [<Route("search")>]
    [<Route("search/{p}/{q}/{sf}")>]
    member me.Search (q    : string,
                      p    : int,
                      sf   : string,
                      sr   : Nullable<Boolean>,
                      from : Nullable<DateTime>,
                      till : Nullable<DateTime>) =

        let searchQuery = me.AssembleSearchQuery(q, p, itemsPerPage, sf, sr, from, till)
        searchQuery |> sprintf "Search with %A" |> logger.Debug
        let result = (searchQuery |> storageReader.SearchByField)
        result

    [<HttpGet>]
    [<Route("{id}/get")>]
    member me.GetById (id: Guid) = 
        storageReader.SearchByField(
            { Expression = "id : " + id.ToString()
              Filter     = None
              Skip       = None
              Sort       = None
              Take       = Some 1 }
        )

    [<HttpGet>]
    [<Route("count-all")>]
    member me.CountAll () = storageReader.CountAll()


