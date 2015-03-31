namespace NBlast.Api.Formatting.Jsonp

open System.Net.Http.Formatting
open System.Web.Http

[<AbstractClass; Sealed>]
type HttpConfigurationExtensions private () =
    static member DoNothing() = ignore