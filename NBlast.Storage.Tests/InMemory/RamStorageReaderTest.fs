namespace NBlast.Storage.Tests.InMemory

open System
open System.IO
open Lucene.Net.Store
open NBlast.Storage
open NBlast.Storage.Core
open NBlast.Storage.Core.Extensions
open NBlast.Storage.Core.Index
open NBlast.Storage.Core.Env
open NBlast.Storage.FileSystem
open Xunit
open FluentAssertions

type RamDirectoryProvider() = 
    let directory = lazy(new RAMDirectory() :> Directory)
    interface IDirectoryProvider with 
        member me.Provide() = directory.Value

type RamStorageReaderTest() = 

    [<Fact>]
    member me.``Do somethings``() =
        // Given
        let provider = new RamDirectoryProvider()
        let writer = me.MakeWriter(provider)
        let sut = me.MakeSut(provider, 10)
        let query = {
            SearchQuery.GetOnlyExpression() with 
                Filter = FilterQuery.After(DateTime.Now.AddDays(-4.0)) |> Some 
        }
        // When
//        me.``gimme 6 fake documents``() |> writer.InsertMany
//        let hits = sut.SearchByField query
        // Then
        0 |> ignore

    member private this.``gimme 6 fake documents`` () = 
        [ new LogDocument("sender1", "1 C C", "log", "debug", None, DateTime.Now.AddDays(-1.0) |> Some);
          new LogDocument("sender1", "2 B", "log", "debug", None, DateTime.Now.AddDays(-2.0) |> Some);
          new LogDocument("sender1", "3 C", "log", "debug", None, DateTime.Now.AddDays(-3.0) |> Some);
          new LogDocument("sender2", "4 B", "log", "debug", None, DateTime.Now.AddDays(-4.0) |> Some);
          new LogDocument("sender2", "5 C", "log", "debug", None, DateTime.Now.AddDays(-5.0) |> Some);
          new LogDocument("sender2", "6 B", "log", "debug", None, DateTime.Now.AddDays(-6.0) |> Some); ] 
            |> List.map (fun x -> x :> IStorageDocument)

    member private this.``gimme 15 fake documents`` () = 
        [ new LogDocument("sender1", "1 C C", "log", "debug", None, DateTime.Now.AddDays(-1.0) |> Some);
          new LogDocument("sender1", "2 B", "log", "debug", None, DateTime.Now.AddDays(-2.0) |> Some);
          new LogDocument("sender1", "3 C", "log", "debug", None, DateTime.Now.AddDays(-3.0) |> Some);
          new LogDocument("sender1", "4 B", "log", "debug", None, DateTime.Now.AddDays(-4.0) |> Some);
          new LogDocument("sender1", "5 C", "log", "debug", None, DateTime.Now.AddDays(-5.0) |> Some);
          new LogDocument("sender2", "6 B", "log", "debug", None, DateTime.Now.AddDays(-6.0) |> Some); 
          new LogDocument("sender2", "7 C", "log", "debug", None, DateTime.Now.AddDays(-7.0) |> Some); 
          new LogDocument("sender2", "8 B", "log", "debug", None, DateTime.Now.AddDays(-8.0) |> Some); 
          new LogDocument("sender2", "9 C", "log", "debug", None, DateTime.Now.AddDays(-9.0) |> Some); 
          new LogDocument("sender2", "10 B", "log", "debug", None, DateTime.Now.AddDays(-10.0) |> Some); 
          new LogDocument("sender3", "11 C", "log", "debug", None, DateTime.Now.AddDays(-11.0) |> Some); 
          new LogDocument("sender3", "12 B", "log", "debug", None, DateTime.Now.AddDays(-12.0) |> Some); 
          new LogDocument("sender3", "13 C", "log", "debug", None, DateTime.Now.AddDays(-13.0) |> Some); 
          new LogDocument("sender3", "14 B", "log", "debug", None, DateTime.Now.AddDays(-14.0) |> Some); 
          new LogDocument("sender3", "15 C", "log", "debug", None, DateTime.Now.AddDays(-15.0) |> Some); ] 
            |> List.map (fun x -> x :> IStorageDocument)

    member private me.MakeWriter provider: IStorageWriter = 
        new StorageWriter(provider) :> IStorageWriter

    member private this.MakeSut(provider, ?itemsPerPage) :IStorageReader =
        let paginator = new Paginator() :> IPaginator
        new StorageReader(provider, paginator, itemsPerPage |? 15) :> IStorageReader