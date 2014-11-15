namespace NBlast.Storage.FileSystem

open NBlast.Storage
open NBlast.Storage.Core
open NBlast.Storage.Core.Exceptions
open Lucene.Net.Analysis.Standard
open Lucene.Net.Documents
open Lucene.Net.QueryParsers
open Lucene.Net.Search
open Lucene.Net.Store
open Lucene.Net.Util
open Lucene.Net.Index
open System.IO

type StorageWriter(path: string, ?reopenWhenLockedOp: bool) = 

    static let logger = NLog.LogManager.GetCurrentClassLogger()
    let directory = lazy (
        let openIndex = fun x -> FSDirectory.Open(new DirectoryInfo(x))
        let tempDirectory = openIndex path
        let isLocked = IndexWriter.IsLocked(tempDirectory)
        let reopenWhenLocked = if (reopenWhenLockedOp.IsSome) 
                                then reopenWhenLockedOp.Value 
                                else false

        if (reopenWhenLocked && isLocked) then
            try
                tempDirectory.ClearLock(IndexWriter.WRITE_LOCK_NAME) 
            with :? System.IO.IOException | :? LockObtainFailedException -> 
                raise(new StorageUnlockFailedException(tempDirectory.Directory.FullName))
        else if (isLocked) then
            raise(new StorageLockedException(tempDirectory.Directory.FullName))
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

    interface IStorageWriter with
        member this.InsertOne(document: IStorageDocument) = 
            use analyser = new StandardAnalyzer(Version.LUCENE_30)
            use writer = new IndexWriter(directory.Value, analyser, IndexWriter.MaxFieldLength.UNLIMITED)
            writer.AddDocument(document.ToLuceneDocument())