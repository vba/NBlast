namespace NBlast.Storage.Tests.InMemory

open System
open System.Runtime
open Xunit
open FluentAssertions
open NBlast.Storage
open NBlast.Storage.FileSystem
open NBlast.Storage.Core
open NBlast.Storage.Core.Index
open NBlast.Storage.Core.Env
open NBlast.Storage.Core.Exceptions
open System.IO
open Lucene.Net.Documents
open Lucene.Net.Store
open Lucene.Net.Util
open Lucene.Net.Index
open Lucene.Net.Analysis.Standard

type FakeDocument() = 
    interface IStorageDocument with 
        member this.ToLuceneDocument() = 
            let document = new Document()
            document.Add(new Field("Id", System.Guid.NewGuid().ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED))
            document

type RamStorageWriterTest() = 

    [<Fact>]
    member this.``Do somethings``() =
        // Given
        // When
        // Then
        0 |> ignore

    member private this.MakeSut reopenWhenLocked path :IStorageWriter =
        let directoryProvider = new WriterDirectoryProvider(path, reopenWhenLocked)
        new StorageWriter(directoryProvider) :> IStorageWriter