namespace NBlast.Api.Controllers

open System
open System.Web.Http
open System.Net.Http.Formatting
open NBlast.Storage.Core.Index
open NBlast.Storage.Core
open NBlast.Storage.Core.Extensions
open System.Web.Http.Cors

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
