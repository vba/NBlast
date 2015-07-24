namespace NBlast.Api.Formatting.Rss

open NBlast.Api.Models
open System
open System.Threading.Tasks
open System.Net.Http.Formatting
open System.Net.Http.Headers

type SyndicationLogFeedFormatter() as me = 
    inherit MediaTypeFormatter() 

    static let atomMediaType = "application/atom+xml"
    static let rssMediaType  = "application/rss+xml"

    static let supportedType = fun (t: Type) -> t = typeof<LogModel> || t = typeof<seq<LogModel>>

    do 
       me.SupportedMediaTypes.Add(new MediaTypeHeaderValue(atomMediaType))
       me.SupportedMediaTypes.Add(new MediaTypeHeaderValue(rssMediaType))

    override me.CanReadType tp = supportedType tp
    override me.CanWriteType tp = supportedType tp
    override me.WriteToStreamAsync(tp, value, stream, content, transportContext) =
        Task.Factory.StartNew(fun () -> 1 |> ignore)
