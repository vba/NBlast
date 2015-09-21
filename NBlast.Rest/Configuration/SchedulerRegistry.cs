using FluentScheduler;
using NBlast.Rest.Async;

namespace NBlast.Rest.Configuration
{
    class SchedulerRegistry: Registry
    {
        public SchedulerRegistry(int minutes = 1)
        {
            this.Schedule<QueueProcessingTask>().ToRunNow().AndEvery(minutes).Minutes();
        }
    }
}