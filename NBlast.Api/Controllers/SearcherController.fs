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
    [<Route("search")>]
    [<Route("search/{p}/{q}/{sf}")>]
    member me.Search (q: string, 
                      p: int, 
                      sf: string,
                      sr: Nullable<Boolean>,
                      from: Nullable<DateTime>,
                      till: Nullable<DateTime>) =

        let sf = LogField.ConvertFrom(sf)
        let searchQuery = {
            Expression = q
            Take = itemsPerPage.Value |> Some
            Skip = ((p - 1) * itemsPerPage.Value) |> Some
            Filter = if (from.HasValue && till.HasValue) 
                        then FilterQuery.Between(from.Value, till.Value) |> Some
                     else if (from.HasValue)
                        then FilterQuery.After(from.Value)  |> Some
                     else if (till.HasValue)
                         then FilterQuery.Before(till.Value) |> Some
                     else None
            Sort = if sf.IsNone 
                    then None
                    else { Field = sf.Value 
                           Reverse = if sr.HasValue 
                                        then sr.Value 
                                        else false} |> Some
        }

        let result = storageReader.SearchByField(searchQuery)
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

