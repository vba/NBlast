namespace NBlast.Api.Formatting.Jsonp

open System.Linq
open System.Net.Http
open System.Net.Http.Formatting
open System.Net.Http.Headers

type JsonpQueryStringMapping =
    inherit QueryStringMapping
    new (parameterName: string, mediaType: MediaTypeHeaderValue) = {
        inherit QueryStringMapping(parameterName, "*", mediaType)
    }
    
    new (parameterName: string, mediaType: string) = {
        inherit QueryStringMapping(parameterName, "*", mediaType)
    }

    override me.TryMatchMediaType(request) =
        let queryString = request.RequestUri.ParseQueryString()
        if queryString.Keys.Cast<string>().Any(fun p -> p = me.QueryStringParameterName) 
            then 1.0 
            else 0.0

