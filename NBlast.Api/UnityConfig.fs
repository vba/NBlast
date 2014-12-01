namespace NBlast.Api

open FluentScheduler
open NBlast.Storage.Core
open NBlast.Storage.Core.Index
open NBlast.Storage
open NBlast.Api.Async
open NBlast.Api.Models
open NBlast.Storage.FileSystem
open Microsoft.Practices.Unity
open System.Web.Http.Dependencies
open System.Configuration
open System
open System.IO

module UnityConfig = 
    let private logger = NLog.LogManager.GetCurrentClassLogger()
    let private configReader = new ConfigReader() :> IConfigReader

    let Configure() =
        logger.Debug("Start to configure IoC container")

        let container = new UnityContainer()
        let directoryPath = "NBlast.index.directory_path" |> configReader.Read |> Path.GetFullPath
        let indexDocumentPerTask = "NBlast.index.documents_per_task" |> configReader.ReadAsInt
        
        container.RegisterInstance<IPaginator>(new Paginator()) |> ignore

        container
            .RegisterInstance<IDirectoryProvider>("ReaderDirectoryProvider", 
                                                  new ReaderDirectoryProvider(directoryPath)) |> ignore 
        container
            .RegisterInstance<IDirectoryProvider>("WriterDirectoryProvider", 
                                                  new WriterDirectoryProvider(directoryPath)) |> ignore
        container
            .RegisterInstance<IStorageReader>(
                new StorageReader(container.Resolve<IDirectoryProvider>("ReaderDirectoryProvider"),
                                  container.Resolve<IPaginator>())
            ) |> ignore
        container
            .RegisterInstance<IStorageWriter>(
                new StorageWriter(container.Resolve<IDirectoryProvider>("ReaderDirectoryProvider"))
            ) |> ignore

        container.RegisterInstance<IIndexingQueueKeeper>(new IndexingQueueKeeper()) |> ignore
        container.RegisterInstance<ITask>(
            new QueueProcessingTask(container.Resolve<IIndexingQueueKeeper>(),
                                    container.Resolve<IStorageWriter>(),
                                    indexDocumentPerTask)
        ) |> ignore

        new UnityResolver(container)