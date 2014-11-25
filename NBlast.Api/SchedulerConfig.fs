namespace NBlast.Api

open NBlast.Api.Async
open FluentScheduler
open FluentScheduler.Model
open Microsoft.Practices.Unity

    type TaskFactory(container: IUnityContainer) =
        interface ITaskFactory with
            member me.GetTaskInstance() =
                container.Resolve<ITask>()

    type ScheduleRegistry() as me =
        inherit Registry()
        do
            me.Schedule<QueueProcessingTask>().ToRunNow().AndEvery(1).Minutes() |> ignore
            
    module SchedulerConfig = 
        let Configure(container: IUnityContainer) =
            TaskManager.TaskFactory <- new TaskFactory(container)
            TaskManager.Initialize(new ScheduleRegistry())
