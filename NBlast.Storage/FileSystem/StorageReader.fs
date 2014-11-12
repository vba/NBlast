namespace NBlast.Storage.FileSystem

open System.IO
open Lucene.Net
open Lucene.Net.QueryParsers
open Lucene.Net.Util
open Lucene.Net.Search
open Lucene.Net.Store
open Lucene.Net.Analysis.Standard

type StorageReader(path: string) = 
    static let logger = NLog.LogManager.GetCurrentClassLogger()
    static let version = Version.LUCENE_30

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

    member this.Search fieldName query =
        use indexSearcher = new IndexSearcher(directory.Value, true)
        use analyzer = new StandardAnalyzer(version)

        let query = _parseQuery query (new QueryParser(version, fieldName, analyzer))
        let docs = indexSearcher.Search(query, null, System.Int32.MaxValue, Sort.RELEVANCE).ScoreDocs
        //new QueryParser(Version.LUCENE_30) 
        []

