namespace NBlast.Storage.FileSystem

open System.IO
open System.Linq
open Lucene.Net
open Lucene.Net.Linq
open Lucene.Net.QueryParsers
open Lucene.Net.Util
open Lucene.Net.Search
open Lucene.Net.Store
open Lucene.Net.Analysis.Standard
open Lucene.Net.Linq.Fluent

type LogDocumentHit = 
    { Sender: string
      Error: string
      Message: string
      Boost: float32
      Score: float
      Content: string }

type StorageReader(path: string) = 
    static let logger = NLog.LogManager.GetCurrentClassLogger()
    static let version = Version.LUCENE_30
    static let paginationAmount = 15

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

        // search by single field
        if (!string.IsNullOrEmpty(searchField)) {
            var parser = new QueryParser(Version.LUCENE_30, searchField, analyzer);
            var query = parseQuery(searchQuery, parser);
            var hits = searcher.Search(query, hits_limit).ScoreDocs;
            var results = _mapLuceneToDataList(hits, searcher);
            analyzer.Close();            
            searcher.Dispose();
            return results;
        }
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


    member this.Search (fieldName, query, page) =
        use indexSearcher = new IndexSearcher(directory.Value, true)
        use analyzer = new StandardAnalyzer(version)
        let query = _parseQuery query (new QueryParser(version, fieldName, analyzer))
        let itemsAmount = (page + 1) * paginationAmount
        let topDocs = indexSearcher.Search(query, null, itemsAmount, Sort.RELEVANCE)
        
        let k = seq {
            for i in ((page * paginationAmount) + 1) .. (if (paginationAmount < topDocs.TotalHits) 
                                                            then (page + 1) * paginationAmount 
                                                            else topDocs.TotalHits ) 
            -> i
        }

        // seq {for i in ((page * 10)+1) ..  (if ((page + 1)*10 < total) then (page + 1)*10 else total) -> i} ;;
        //let take = if (totalSkip >= paginationAmount) then paginationAmount else totalSkip

        //seq {for i in skip to  }
        //new QueryParser(Version.LUCENE_30) 
        []

    member this.Search (fieldName, query) = this.Search(fieldName, query, 1)

