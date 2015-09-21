using FluentScheduler;
using Serilog;

namespace NBlast.Rest.Async
{
    public interface IQueueProcessingTask : ITask
    {
    }

    public class QueueProcessingTask: IQueueProcessingTask
    {
        private static readonly ILogger Logger = Log.Logger.ForContext<QueueProcessingTask>();
        public void Execute()
        {
            Logger.Verbose("Task.Executed");
        }
    }
}