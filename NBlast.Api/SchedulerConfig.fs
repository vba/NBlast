namespace NBlast.Api

open NBlast.Storage.Core
open NBlast.Api.Async
open FluentScheduler
open FluentScheduler.Model
open Microsoft.Practices.Unity
open System

    type TaskFactory(container: IUnityContainer) =
        interface ITaskFactory with
            member me.GetTaskInstance() =
                container.Resolve<ITask>()

    type ScheduleRegistry(?minutes: int) as me =
        inherit Registry()
        do
            let minutes = defaultArg minutes 1 
            me.Schedule<QueueProcessingTask>().ToRunNow().AndEvery(minutes).Minutes() |> ignore
            
    module SchedulerConfig = 
        let private logger = NLog.LogManager.GetCurrentClassLogger()

        let private onTaskException s (e: UnhandledExceptionEventArgs) =
            logger.Fatal(e.ExceptionObject) |> ignore

        let Configure(container: IUnityContainer) =
            logger.Debug("Start to configure task scheduler")

            let configReader = container.Resolve<IConfigReader>()
            let minutes      = configReader.ReadAsInt("NBlast.indexing.scheduler.run_every_minutes")
            let registry     = new ScheduleRegistry(minutes)
            let eventHandler = new GenericEventHandler<TaskExceptionInformation, UnhandledExceptionEventArgs>(onTaskException)

            logger.Debug("Schedule task runner for every {0} minute(s)", minutes)

            TaskManager.TaskFactory <- new TaskFactory(container)
            TaskManager.add_UnobservedTaskException(eventHandler)
            TaskManager.Initialize(registry)

