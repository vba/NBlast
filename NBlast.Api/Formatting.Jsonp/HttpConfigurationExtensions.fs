namespace NBlast.Api.Formatting.Jsonp

open System.Net.Http.Formatting
open System.Web.Http
open System

[<AutoOpen>]
module HttpConfigurationExtensions =
    type HttpConfiguration with 
        member me.AddJsonpFormatter(?jsonFormatter: MediaTypeFormatter, ?callbackQueryParameter: String) =
            let formatter = match (jsonFormatter, callbackQueryParameter) with
                            | (Some(x), Some(y)) -> new JsonpMediaTypeFormatter(x, y)
                            | (Some(x), _)       -> new JsonpMediaTypeFormatter(x)
                            | (_, Some(x))       -> new JsonpMediaTypeFormatter(me.Formatters.JsonFormatter, x)
                            | _                  -> new JsonpMediaTypeFormatter(me.Formatters.JsonFormatter)
            me.Formatters.Add(formatter) |> ignore