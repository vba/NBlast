﻿namespace NBlast.Storage.FileSystem

open NBlast.Storage
open NBlast.Storage.Core.Index
open NBlast.Storage.Core.Exceptions
open Lucene.Net.Analysis.Standard
open Lucene.Net.Documents
open Lucene.Net.QueryParsers
open Lucene.Net.Search
open Lucene.Net.Store
open Lucene.Net.Util
open Lucene.Net.Index
open System.IO

type StorageWriter(directoryProvider: IDirectoryProvider) = 

    static let logger = NLog.LogManager.GetCurrentClassLogger()
    do
        logger.Debug("Initialization is over")

    interface IStorageWriter with
        member this.InsertMany(documents: seq<IStorageDocument>) =
            documents |> Seq.length |> sprintf "Indexing %d documents" |> logger.Debug
            use directory = directoryProvider.Provide()
            use analyser  = new StandardAnalyzer(Version.LUCENE_30)
            use writer    = new IndexWriter(directory, analyser, IndexWriter.MaxFieldLength.UNLIMITED)

            documents |> Seq.iter (fun x -> writer.AddDocument(x.ToLuceneDocument()))

        member this.InsertOne(document: IStorageDocument) = 
            (this :> IStorageWriter).InsertMany([ document ])
