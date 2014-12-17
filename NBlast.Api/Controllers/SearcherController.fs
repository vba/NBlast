namespace NBlast.Api.Controllers


open System.Web.Http
open System.Net.Http.Formatting
open NBlast.Storage.Core.Index
open System.Web.Http.Cors

[<RoutePrefix("api/searcher")>]
[<EnableCors("*", "X-NBLAST-CLIENT", "GET")>]
type SearcherController(storageReader: IStorageReader) = 
    inherit ApiController()

    static let logger = NLog.LogManager.GetCurrentClassLogger()

    [<HttpGet>]
    member me.Search (q: string) =
        let result = storageReader.SearchByField(SearchQuery.GetOnlyExpression q)
        //result |> sprintf "search result: %A" |> logger.Debug
        result

    [<HttpGet>]
    [<Route("count-all")>]
    member me.CountAll () =
        storageReader.CountAll()

