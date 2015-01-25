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

    let itemsPerPage = lazy("NBlast.search.hits_per_page" |> configReader.ReadAsInt)

    [<HttpGet>]
    [<Route("search")>]
    [<Route("search/{q}")>]
    member me.Search (q: string) = me.Search(q, 1)

    [<HttpGet>]
    [<Route("search")>]
    [<Route("search/{p}/{q}")>]
    member me.Search (q: string, p: int) =
        let searchQuery = {
            SearchQuery.GetOnlyExpression q 
            with Take = Some itemsPerPage.Value 
                 Skip = Some ((p - 1) * itemsPerPage.Value)
        }
        let result = storageReader.SearchByField(searchQuery)
        //result |> sprintf "search result: %A" |> logger.Debug
        result

    [<HttpGet>]
    [<Route("{id}/get")>]
    member me.GetById (id: Guid) = 
        let result = storageReader.SearchByField({
                                                    Expression = "id : " + id.ToString()
                                                    Filter     = None
                                                    Skip       = None
                                                    Sort       = None
                                                    Take       = Some 1
                                                 })
        result

    [<HttpGet>]
    [<Route("count-all")>]
    member me.CountAll () =
        storageReader.CountAll()

