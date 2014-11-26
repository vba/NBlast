namespace NBlast.Api.Controllers


open System.Web.Http
open System.Net.Http.Formatting
open NBlast.Storage.Core.Index

//[<RoutePrefix("api/log-search")>]
type SearcherController(storageReader: IStorageReader) = 
    inherit ApiController()

    static let logger = NLog.LogManager.GetCurrentClassLogger()

    [<HttpGet>]
    member me.Search (q: string) =
        let result = storageReader.SearchByField(SearchQuery.GetOnlyExpression q)
        result |> sprintf "search result: %A" |> logger.Debug
        result

