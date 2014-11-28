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

    type AppSettingNotFoundException = 
        inherit InvalidOperationException
        new (key) = { inherit InvalidOperationException(sprintf "App setting not found for key %s" key) } 

    type AppSettingTryCastException = 
        inherit InvalidOperationException
        new (key, ``type``) = { inherit InvalidOperationException(sprintf "Cannot conver setting %s to %A" key ``type``) } 

    module UnityConfig = 
        let private logger = NLog.LogManager.GetCurrentClassLogger()

        let private ReadConfig (key: string) =
            try
                ConfigurationManager.AppSettings.[key] |> Environment.ExpandEnvironmentVariables
            with
            | _ -> raise (new AppSettingNotFoundException(key))

        let private ReadConfigAsInt key =
            match ReadConfig key |> Int32.TryParse with
            | (false, _) -> raise (new AppSettingTryCastException(key, int32.GetType()))
            | (true, value) -> value

        let Configure() =
            logger.Debug("Start to configure IoC container")

            let container = new UnityContainer()
            let directoryPath = "NBlast.directoryPath" |> ReadConfig |> Path.GetFullPath
            
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
                new QueueProcessingTask(container.Resolve<IIndexingQueueKeeper>(), container.Resolve<IStorageWriter>())
            ) |> ignore

            new UnityResolver(container)