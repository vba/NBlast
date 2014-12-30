namespace NBlast.Api.Controllers

open System
open System.Web.Http
open System.Net.Http.Formatting
open NBlast.Storage.Core.Index
open System.Web.Http.Cors

[<RoutePrefix("api/searcher")>]
[<EnableCors("*", "*", "GET")>]
type SearcherController(storageReader: IStorageReader) = 
    inherit ApiController()

    static let logger = NLog.LogManager.GetCurrentClassLogger()

    [<HttpGet>]
    member me.Search (q: string) =
        let result = storageReader.SearchByField(SearchQuery.GetOnlyExpression q)
        //result |> sprintf "search result: %A" |> logger.Debug
        result

    [<HttpGet>]
    [<Route("{id}/get")>]
    member me.GetById (id: Guid) = 
        let result = storageReader.SearchByField({
                                                    Expression = "id : " + id.ToString()
                                                    Filter     = None
                                                    Skip       = None
                                                    Take       = Some 1
                                                 })
        result

    [<HttpGet>]
    [<Route("count-all")>]
    member me.CountAll () =
        storageReader.CountAll()

