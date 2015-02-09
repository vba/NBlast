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
        searchQuery |> sprintf "Simple search with %A" |> logger.Debug
        let result = storageReader.SearchByField(searchQuery)
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

        let searchQuery = me.AssembleSearchQuery(q, p, sf, sr, from, till)
        searchQuery |> sprintf "Complex search with %A" |> logger.Debug
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

    member private me.CombineExpression q = 
        SearchQuery.GetOnlyExpression(q)

    member private me.CombinePage p query = 
        { query with Take = itemsPerPage.Value |> Some 
                     Skip = ((p - 1) * itemsPerPage.Value) |> Some }

    member private me.CombineSort sf (sr: Nullable<Boolean>) query = 
        let sf = LogField.ConvertFrom(sf)
        { query with Sort = if sf.IsNone 
                            then None
                            else { Field = sf.Value 
                                   Reverse = if sr.HasValue 
                                                then sr.Value 
                                                else false} |> Some }

    member private me.CombineFilter (from: Nullable<DateTime>) 
                                    (till: Nullable<DateTime>) 
                                    query = 
        { query with Filter = if (from.HasValue && till.HasValue) 
                                then FilterQuery.Between(from.Value, till.Value) |> Some
                              else if (from.HasValue)
                                then FilterQuery.After(from.Value)  |> Some
                              else if (till.HasValue)
                                then FilterQuery.Before(till.Value) |> Some
                              else None }

    member private me.AssembleSearchQuery (q, p, sf, sr, from, till) = 
        q |> (me.CombineExpression 
                >> (me.CombinePage p) 
                >> (me.CombineSort sf sr) 
                >> (me.CombineFilter from till))

        

