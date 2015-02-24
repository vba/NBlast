namespace NBlast.Api.Controllers

open System
open System.Web.Http
open System.Net.Http.Formatting
open NBlast.Storage.Core.Index
open NBlast.Storage.Core
open NBlast.Storage.Core.Extensions
open System.Web.Http.Cors
open Newtonsoft.Json.Linq

[<RoutePrefix("api/dashboard")>]
[<EnableCors("*", "*", "GET")>]
type DashboardController(storageReader: IStorageReader,
                         configReader: IConfigReader) =

    inherit ApiController()

    static let logger = NLog.LogManager.GetCurrentClassLogger()

    [<HttpGet>]
    [<Route("group-by/{field}")>]
    member me.GroupByField (field) = me.GroupByField(field, 10)

    [<HttpGet>]
    [<Route("group-by/{field}/{limit}")>]
    member me.GroupByField (field, limit) =
        let logField = field |> LogField.ConvertFrom
        if logField.IsNone then
            field |> sprintf "Invalid field name %s" |> me.BadRequest :> IHttpActionResult
        else
            let facets = (logField.Value |> storageReader.GroupWith)
            me.Ok({facets with Facets = facets.Facets |> Seq.truncate limit }) :> IHttpActionResult


    [<HttpGet>]
    [<Route("levels-per-day/{days}")>]
    member me.``Levels per day`` (days) =
        let result       = new JObject()
        let logField     = LogField.Level
        let from         = DateTime.Now.Date.AddDays(days)
        let till         = from.AddHours(23.0).AddMinutes(59.0).AddSeconds(59.0)
        let filter       = FilterQuery.Between(from, till) |> Some
        let query        = fun x -> {SearchQuery.GetOnlyExpression(x) with Filter = filter; Take = Some(1)}
        let searchByTerm = fun x -> storageReader.SearchByTerm(logField, query(x)).Total

        result.Add("trace", new JValue("trace" |> searchByTerm))
        result.Add("info", new JValue("info" |> searchByTerm))
        result.Add("debug", new JValue("debug" |> searchByTerm))
        result.Add("warn", new JValue("warn" |> searchByTerm))
        result.Add("error", new JValue("error" |> searchByTerm))
        result.Add("fatal", new JValue("fatal" |> searchByTerm))

        result