namespace NBlast.Api

open NBlast.Api.Async
open FluentScheduler
open FluentScheduler.Model
open Microsoft.Practices.Unity
open System

    type TaskFactory(container: IUnityContainer) =
        interface ITaskFactory with
            member me.GetTaskInstance() =
                container.Resolve<ITask>()

    type ScheduleRegistry() as me =
        inherit Registry()
        do
            me.Schedule<QueueProcessingTask>().ToRunNow().AndEvery(1).Minutes() |> ignore
            
    module SchedulerConfig = 
        let private logger = NLog.LogManager.GetCurrentClassLogger()

        let private onTaskException s (e: UnhandledExceptionEventArgs) =
            logger.Fatal(e.ExceptionObject) |> ignore

        let Configure(container: IUnityContainer) =
            logger.Debug("Start to configure task scheduler")

            TaskManager.TaskFactory <- new TaskFactory(container)
            TaskManager.add_UnobservedTaskException(new GenericEventHandler<TaskExceptionInformation, UnhandledExceptionEventArgs>(onTaskException))
            TaskManager.Initialize(new ScheduleRegistry())

