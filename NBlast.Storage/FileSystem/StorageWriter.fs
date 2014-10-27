namespace NBlast.Storage

open NBlast.Storage.Core
open Lucene.Net.Analysis.Standard
open Lucene.Net.Documents
open Lucene.Net.QueryParsers
open Lucene.Net.Search
open Lucene.Net.Store
open Lucene.Net.Util
open Lucene.Net.Index
open System.IO

type StorageWriter(reopenWhenLocked: bool, path: string) = 

    static let logger = NLog.LogManager.GetCurrentClassLogger()
    let directory = lazy (
        let openIndex = fun x -> FSDirectory.Open(new DirectoryInfo(x))
        let tempDirectory = openIndex path

        if (reopenWhenLocked && IndexWriter.IsLocked(tempDirectory)) then 
            IndexWriter.Unlock(tempDirectory)
            let lockFile = Path.Combine(tempDirectory.Directory.FullName, "write.lock")
            if (File.Exists(lockFile)) then File.Delete(lockFile)
        else if (IndexWriter.IsLocked(tempDirectory)) then
            let message = sprintf "Index directory is already locked in %s" tempDirectory.Directory.FullName
            raise(new System.InvalidOperationException(message))
        tempDirectory
    )
    do
        logger.Debug("Initialization is over")

    (*
    var doc = new Document();

    // add lucene fields mapped to db fields
    doc.Add(new Field("Id", sampleData.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
    doc.Add(new Field("Name", sampleData.Name, Field.Store.YES, Field.Index.ANALYZED));
    doc.Add(new Field("Description", sampleData.Description, Field.Store.YES, Field.Index.ANALYZED));

    // add entry to index
    writer.AddDocument(doc);
    *)
    member this.InsertOne() = 
        let analyser = new StandardAnalyzer(Version.LUCENE_30)
        use writer = new IndexWriter(directory.Value, analyser, IndexWriter.MaxFieldLength.UNLIMITED)
        let document = new Document()
        document.Add(new Field("Id", System.Guid.NewGuid().ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED))
        writer.AddDocument(document)
        analyser.Close()
        writer.Dispose()