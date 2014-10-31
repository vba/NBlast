﻿namespace NBlast.Storage.FileSystem

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

type StorageWriter(reopenWhenLocked: bool, path: string) = 

    static let logger = NLog.LogManager.GetCurrentClassLogger()
    let directory = lazy (
        let openIndex = fun x -> FSDirectory.Open(new DirectoryInfo(x))
        let tempDirectory = openIndex path
        let isLocked = IndexWriter.IsLocked(tempDirectory)

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
    member this.InsertOne() = 
        let analyser = new StandardAnalyzer(Version.LUCENE_30)
        use writer = new IndexWriter(directory.Value, analyser, IndexWriter.MaxFieldLength.UNLIMITED)
        let document = new Document()
        document.Add(new Field("Id", System.Guid.NewGuid().ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED))
        writer.AddDocument(document)
        analyser.Close()
        writer.Dispose()