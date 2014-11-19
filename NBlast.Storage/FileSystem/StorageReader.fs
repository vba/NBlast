namespace NBlast.Storage.FileSystem

open System
open System.IO
open System.Linq
open System.Diagnostics
open Lucene.Net
open NBlast.Storage
open NBlast.Storage.Core.Extensions
open NBlast.Storage.Core
open NBlast.Storage.Core.Index
open Lucene.Net.QueryParsers
open Lucene.Net.Util
open Lucene.Net.Search
open Lucene.Net.Documents
open Lucene.Net.Store
open Lucene.Net.Analysis.Standard
open Lucene.Net.Index
open FSharp.Collections.ParallelSeq

type StorageReader(path: string, ?itemsPerPage: int) = 
    static let logger = NLog.LogManager.GetCurrentClassLogger()
    static let version = Version.LUCENE_30
    
    let itemsPerPage = itemsPerPage |? 15
    let paginator = new Paginator() :> IPaginator

    static let _parseQuery = fun query (parser: QueryParser) ->
        try
            parser.Parse(query)
        with
            | :? ParseException -> parser.Parse(QueryParser.Escape(query.Trim()))

    let directory = lazy(FSDirectory.Open(new DirectoryInfo(path)))
    let indexReader = lazy(IndexReader.Open(directory.Value, true))

    member private this.HitToDocument (pair: Document * float32 option) = 
        let doc = fst pair
        let score = snd pair
        { 
          Score     = score; 
          Boost     = doc.Boost;
          Sender    = doc.Get(LogField.Sender.GetName());
          Error     = doc.Get(LogField.Error.GetName());
          Message   = doc.Get(LogField.Message.GetName());
          Logger    = doc.Get(LogField.Logger.GetName());
          Level     = doc.Get(LogField.Level.GetName());
          CreatedAt = DateTools.StringToDate(doc.Get(LogField.CreatedAt.GetName()));
        }

    member private me.GroupByFields() =
        
        []

    interface IStorageReader with
        member this.SearchByField query ?skipOp ?takeOp =
            use indexSearcher = new IndexSearcher(directory.Value, true)
            use analyzer = new StandardAnalyzer(version)
            let (skip, take) = (skipOp |? 0, takeOp |? itemsPerPage)
            let parser = new MultiFieldQueryParser(version, LogField.Names, analyzer)
            let query = _parseQuery query parser
            let sw = new Stopwatch()

            sw.Start()
            let topDocs = indexSearcher.Search(query, null, skip + take)
            sw.Stop()

            let getHit = fun (index) -> 
                let sd    = topDocs.ScoreDocs.[index - 1]
                let score = if (Single.IsNaN(sd.Score)) then None else Some(sd.Score)
                (indexSearcher.Doc(sd.Doc), score)

            let hitsSection = paginator.GetFollowingSection skip take topDocs.TotalHits 
            let hits = hitsSection 
                        |> Seq.map (getHit >> this.HitToDocument) // TODO Weak place, needs to be processed with parallel sequences
                        |> Seq.toList 

            { Hits          = hits; 
              Total         = topDocs.TotalHits; 
              QueryDuration = sw.ElapsedMilliseconds }

        member me.FindAll ?skipOp ?takeOp = 
            (me :> IStorageReader).SearchByField "*:*" skipOp takeOp

        member me.CountAll() = indexReader.Value.NumDocs()