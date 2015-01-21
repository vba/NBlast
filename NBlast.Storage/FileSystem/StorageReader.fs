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

type StorageReader (directoryProvider: IDirectoryProvider,
                    paginator: IPaginator, 
                    ?itemsPerPage: int) = 

    static let logger = NLog.LogManager.GetCurrentClassLogger()
    static let version = Version.LUCENE_30
    
    let itemsPerPage = itemsPerPage |? 10
    //let paginator = new Paginator() :> IPaginator

    static let _parseQuery = fun query (parser: QueryParser) ->
        try
            parser.Parse(query)
        with
            | :? ParseException -> parser.Parse(QueryParser.Escape(query.Trim()))


    member private this.HitToDocument (pair: Document * float32 option) = 
        let doc = fst pair
        let score = snd pair
        { 
          Score     = match score with 
                        | Some x -> x 
                        | _ -> 0.0f
          Boost     = doc.Boost;
          Id        = doc.Get(LogField.Id.GetName());
          Sender    = doc.Get(LogField.Sender.GetName());
          Error     = doc.Get(LogField.Error.GetName());
          Message   = doc.Get(LogField.Message.GetName());
          Logger    = doc.Get(LogField.Logger.GetName());
          Level     = doc.Get(LogField.Level.GetName());
          CreatedAt = DateTools.StringToDate(doc.Get(LogField.CreatedAt.GetName()));
        }

    interface IStorageReader with
        member me.GroupWith (field: LogField) =
            use directory = directoryProvider.Provide()
            use indexReader = IndexReader.Open(directory, true)
            use analyzer = new StandardAnalyzer(version)
            use facetedSearcher = new SimpleFacetedSearch(indexReader, LogField.Sender.GetName())
            let parser = new MultiFieldQueryParser(version, LogField.Names, analyzer)
            let query = _parseQuery "*:*" parser
            let sw = new Stopwatch()

            sw.Start()
            let hits = facetedSearcher.Search(query)
            sw.Stop()

            let facets = hits.HitsPerFacet 
                         |> Seq.map (fun x -> {Name = x.Name.ToString(); Count = x.HitCount }) 
                         |> Seq.toList

            { Facets        = facets
              QueryDuration = sw.ElapsedMilliseconds }

        member this.SearchByField searchQuery =
            use directory = directoryProvider.Provide()
            use indexSearcher = new IndexSearcher(directory, true)
            use analyzer = new StandardAnalyzer(version)
            let (skip, take) = (searchQuery.Skip |? 0, searchQuery.Take |? itemsPerPage)
            let parser = new MultiFieldQueryParser(version, LogField.Names, analyzer)
            let query = _parseQuery searchQuery.Expression parser
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
            let query = { (SearchQuery.GetOnlyExpression "*:*") 
                            with Skip = skipOp
                                 Take = takeOp }

            (me :> IStorageReader).SearchByField query

        member me.CountAll() = 
            use directory = directoryProvider.Provide()
            use indexReader = IndexReader.Open(directory, true)
            indexReader.NumDocs()