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

        let searchQuery = me.AssembleSearchQuery(q, p, sf, sr, from, till)
        searchQuery |> sprintf "Search with %A" |> logger.Debug
        let result = (searchQuery |> storageReader.SearchByField)
        result

    [<HttpGet>]
    [<Route("term-search")>]
    member me.TermSearch (t, q) = me.TermSearch(t, q, 1)

    [<HttpGet>]
    [<Route("term-search")>]
    member me.TermSearch (t, q, p) =
        me.TermSearch (t, q, p, null, new Nullable<_>(), new Nullable<_>(), new Nullable<_>())

    [<HttpGet>]
    [<Route("term-search")>]
    member me.TermSearch (t    : string,
                          q    : string,
                          p    : int,
                          sf   : string,
                          sr   : Nullable<Boolean>,
                          from : Nullable<DateTime>,
                          till : Nullable<DateTime>) =

        let logField = t |> LogField.ConvertFrom
        if logField.IsNone then
            t |> sprintf "Invalid field name %s" |> me.BadRequest :> IHttpActionResult
        else
            let searchQuery = me.AssembleSearchQuery(q, p, sf, sr, from, till)
            searchQuery |> sprintf "Term search with term %s and %A" t |> logger.Debug
            let result = (searchQuery |> storageReader.SearchByTerm logField.Value) 
            me.Ok(result) :> IHttpActionResult

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

    member private me.CombineExpression q = 
        SearchQuery.GetOnlyExpression(q)

    member private me.CombinePage p query = 
        { query with Take = itemsPerPage.Value |> Some 
                     Skip = ((p - 1) * itemsPerPage.Value) |> Some }

    member private me.CombineSort sf (sr: Nullable<Boolean>) query = 
        let sf = LogField.ConvertFrom(sf)
        { query with Sort = if sf.IsNone then None
                            else { Field   = sf.Value 
                                   Reverse = if sr.HasValue 
                                             then sr.Value 
                                             else false} |> Some }

    member private me.CombineFilter (from: Nullable<DateTime>) 
                                    (till: Nullable<DateTime>) 
                                    query = 
        { query with 
            Filter = 
                if (from.HasValue && till.HasValue) 
                    then FilterQuery.Between(from.Value, till.Value) |> Some
                else if (from.HasValue)
                    then FilterQuery.After(from.Value)  |> Some
                else if (till.HasValue)
                    then FilterQuery.Before(till.Value) |> Some
                else None
        }

    member private me.AssembleSearchQuery (q, p, sf, sr, from, till) = 
        q |> (me.CombineExpression 
                >> (me.CombinePage p) 
                >> (me.CombineSort sf sr) 
                >> (me.CombineFilter from till))
