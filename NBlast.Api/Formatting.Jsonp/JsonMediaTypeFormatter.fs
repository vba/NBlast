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
                             ?queryParameter        : string) as me =
    inherit MediaTypeFormatter()

    let callbackQueryParameter = defaultArg queryParameter "callback"

    static let applicationJavascript = new MediaTypeHeaderValue("application/javascript")
    static let applicationJsonp      = new MediaTypeHeaderValue("application/json-p")
    static let textJavascript        = new MediaTypeHeaderValue("text/javascript")

    do
        me.SupportedMediaTypes.Add(applicationJavascript)
        me.SupportedMediaTypes.Add(applicationJsonp)
        me.SupportedMediaTypes.Add(textJavascript)

        for encoding in mediaTypeFormatter.SupportedEncodings do
            me.SupportedEncodings.Add(encoding)

        me.MediaTypeMappings.Add(new JsonpQueryStringMapping(callbackQueryParameter, textJavascript))
    
    override me.CanReadType(tp) = false
    override me.CanWriteType(tp) = mediaTypeFormatter.CanWriteType(tp)

    override me.GetPerRequestFormatterInstance(tp, rq, mt) =
        match me.GetJsonpCallback request callbackQueryParameter with
        | Some(callback) -> new JsonpMediaTypeFormatter(rq, mediaTypeFormatter, callback, callbackQueryParameter) :> MediaTypeFormatter
        | _ -> raise(new InvalidOperationException("No callback"))


    member private me.GetJsonpCallback request queryParameter : string option = 
        if request.Method = HttpMethod.Get 
        then 
            request.GetQueryNameValuePairs() 
                |> Seq.filter (fun x -> x.Key.Equals(queryParameter, StringComparison.OrdinalIgnoreCase))
                |> Seq.map (fun x -> x.Value)
                |> Seq.tryPick (fun x -> Some x)
        else 
            None
        