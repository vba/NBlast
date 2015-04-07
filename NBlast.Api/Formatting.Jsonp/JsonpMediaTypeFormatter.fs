namespace NBlast.Api.Formatting.Jsonp

open System
open System.IO
open System.Linq
open System.Net
open System.Net.Http
open System.Net.Http.Formatting
open System.Net.Http.Headers
open System.Threading.Tasks


type JsonpMediaTypeFormatter(mediaTypeFormatter     : MediaTypeFormatter,
                             ?callback              : string,
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

    override me.GetPerRequestFormatterInstance(tp, request, mt) =
        match JsonpMediaTypeFormatter.GetJsonpCallback request callbackQueryParameter with
        | Some(callback) -> new JsonpMediaTypeFormatter(mediaTypeFormatter, callback, callbackQueryParameter) :> MediaTypeFormatter
        | _ -> raise(new InvalidOperationException("No callback"))


    override me.WriteToStreamAsync(tp, value, stream, content, transportContext) =
        let headers  = if content = null then content.Headers else null
        let encoding = me.SelectCharacterEncoding(headers)
        let callback = defaultArg callback ""

        if String.IsNullOrEmpty(callback) then raise(new InvalidOperationException("No callback"))

        use writer = new StreamWriter(stream, encoding, 4096, true)
        
        writer.Write(callback + "(")
        writer.Flush()
        mediaTypeFormatter.WriteToStreamAsync(tp, value, stream, content, transportContext).RunSynchronously()
        writer.Write(")")
        writer.Flush()
        new Task(fun x -> ignore())


    static member GetJsonpCallback request queryParameter : string option = 
        if request.Method = HttpMethod.Get 
        then 
            request.GetQueryNameValuePairs() 
                |> Seq.filter (fun x -> x.Key.Equals(queryParameter, StringComparison.OrdinalIgnoreCase))
                |> Seq.map (fun x -> x.Value)
                |> Seq.tryPick (fun x -> Some x)
        else 
            None
        