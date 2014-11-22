namespace NBlast.Api.Controllers


open System.Web.Http
open System.Net.Http.Formatting
open NBlast.Storage.Core.Index

//[<RoutePrefix("api/log-search")>]
type SearcherController(storageReader: IStorageReader) = 
    inherit ApiController()

    [<HttpGet()>]
    member me.Search (q: string) =
        let result = storageReader.SearchByField(SearchQuery.GetOnlyExpression q)
        printfn "search result: %A" result
        result

