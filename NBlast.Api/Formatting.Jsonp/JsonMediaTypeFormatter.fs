namespace NBlast.Api.Formatting.Jsonp

open System
open System.IO
open System.Linq
open System.Net
open System.Net.Http
open System.Net.Http.Formatting
open System.Net.Http.Headers
open System.Threading.Tasks
(*
    internal class JsonpQueryStringMapping : QueryStringMapping
    {
        public JsonpQueryStringMapping(string queryStringParameterName, MediaTypeHeaderValue mediaType)
            : base(queryStringParameterName, "*", mediaType)
        {
        }

        public JsonpQueryStringMapping(string queryStringParameterName, string mediaType)
            : base(queryStringParameterName, "*", mediaType)
        {
        }

        public override double TryMatchMediaType(HttpRequestMessage request)
        {
            var queryString = request.RequestUri.ParseQueryString();
            return queryString.Keys.Cast<string>().Any(p => p == QueryStringParameterName) ? 1.0 : 0.0;
        }
    }
*)

type JsonpMediaTypeFormatter(request                : HttpRequestMessage, 
                             mediaTypeFormatter     : MediaTypeFormatter,
                             callback               : string,
                             ?callbackQueryParameter: string) as me =
    inherit MediaTypeFormatter()

    static let applicationJavaScript = new MediaTypeHeaderValue("application/javascript")
    static let applicationJsonp      = new MediaTypeHeaderValue("application/json-p")
    static let textJavaScript        = new MediaTypeHeaderValue("text/javascript")

    do
        me.SupportedMediaTypes.Add(applicationJavaScript)
        me.SupportedMediaTypes.Add(applicationJsonp)
        me.SupportedMediaTypes.Add(textJavaScript)
    
    override me.CanReadType(tp) = false
    override me.CanWriteType(tp) = mediaTypeFormatter.CanWriteType(tp)

    override me.GetPerRequestFormatterInstance(tp, rq, mt) =
        new JsonpMediaTypeFormatter(rq, mediaTypeFormatter, callback, defaultArg callbackQueryParameter "") :> MediaTypeFormatter