namespace NBlast.Api.Formatting.Rss

open NBlast.Api.Models
open System
open System.Xml
open System.Collections
open System.ServiceModel.Syndication
open System.Threading.Tasks
open System.Net.Http.Formatting
open System.Net.Http.Headers

type SyndicationLogFeedFormatter() as me = 
    inherit MediaTypeFormatter() 

    static let syndicationTitle = ""
    static let atomMediaType = "application/atom+xml"
    static let rssMediaType  = "application/rss+xml"

    static let supportedType = fun (t: Type) -> t = typeof<LogModel> || t = typeof<seq<LogModel>>

    do 
       me.SupportedMediaTypes.Add(new MediaTypeHeaderValue(atomMediaType))
       me.SupportedMediaTypes.Add(new MediaTypeHeaderValue(rssMediaType))

    override me.CanReadType tp = supportedType tp
    override me.CanWriteType tp = supportedType tp
    override me.WriteToStreamAsync(tp, value, stream, content, transportContext) =
        Task.Factory.StartNew(fun () -> 
            if supportedType(tp)
                then me.BuildSyndicationFeed value stream content.Headers.ContentType.MediaType
        )

    member private me.BuildSyndicationFeed value stream contentType =
        let feed = new SyndicationFeed(Title = new TextSyndicationContent(syndicationTitle))
        feed.Items <- if (value.GetType() = typeof<seq<LogModel>>)
                      then (value :?> seq<LogModel>) |> Seq.map (fun x -> me.BuildSyndicationItem(x))
                      else [|me.BuildSyndicationItem(value :?> LogModel)|] |> Array.toSeq

        use writer = XmlWriter.Create(stream)
        let formatter = if contentType = atomMediaType 
                        then new Atom10FeedFormatter(feed) :> SyndicationFeedFormatter
                        else new Rss20FeedFormatter(feed) :> SyndicationFeedFormatter

        ignore 1

    member private me.BuildSyndicationItem (model: LogModel) = 
        new SyndicationItem(Title           = new TextSyndicationContent(model.Sender),
                            Content         = new TextSyndicationContent(model.Message),
                            LastUpdatedTime = new DateTimeOffset(model.CreatedAt.Value))