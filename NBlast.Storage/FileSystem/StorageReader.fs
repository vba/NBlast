namespace NBlast.Storage.FileSystem

open System
open System.IO
open System.Linq
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
(*
private static IEnumerable<SampleData> _search
    (string searchQuery, string searchField = "") {
    // validation
    if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", ""))) return new List<SampleData>();

    // set up lucene searcher
    using (var searcher = new IndexSearcher(_directory, false)) {
        var hits_limit = 1000;
        var analyzer = new StandardAnalyzer(Version.LUCENE_30);

        // search by multiple fields (ordered by RELEVANCE)
        else {
            var parser = new MultiFieldQueryParser
                (Version.LUCENE_30, new[] { "Id", "Name", "Description" }, analyzer);
            var query = parseQuery(searchQuery, parser);
            var hits = searcher.Search
            (query, null, hits_limit, Sort.RELEVANCE).ScoreDocs;
            var results = _mapLuceneToDataList(hits, searcher);
            analyzer.Close();            
            searcher.Dispose();
            return results;
        }
    }
} 
*)

    member this.GetAllRecords () =
        []

    member private this.HitToDocument (pair: Document * float32 option) = 
        let doc = fst pair
        let score = snd pair
        { 
          Score     = score; 
          Boost     = doc.Boost;
          Sender    = doc.Get("sender");
          Error     = doc.Get("error");
          Message   = doc.Get("message");
          Logger    = doc.Get("logger");
          Level     = doc.Get("level");
          CreatedAt = DateTools.StringToDate(doc.Get("createdAt"));
        }
        
    interface IStorageReader with
        member this.Search fieldName query ?skipOp ?takeOp =
            use indexSearcher = new IndexSearcher(directory.Value, true)
            use analyzer = new StandardAnalyzer(version)
            let query = _parseQuery query (new QueryParser(version, fieldName, analyzer))
            let skip = skipOp |? 0
            let take = takeOp |? itemsPerPage
            let topDocs = indexSearcher.Search(query, null, skip + take)
            let getHit = fun (index) -> 
                let sd = topDocs.ScoreDocs.[index - 1]
                let score = if (Single.IsNaN(sd.Score)) then None else Some(sd.Score)
                (indexSearcher.Doc(sd.Doc), score)

            let hitsSection = paginator.GetFollowingSection skip take topDocs.TotalHits 

            hitsSection |> Seq.map (getHit >> this.HitToDocument) |> Seq.toList