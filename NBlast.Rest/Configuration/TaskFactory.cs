using FluentScheduler;
using NBlast.Rest.Async;
using Ninject;

namespace NBlast.Rest.Configuration
{
    class TaskFactory: ITaskFactory
    {
        private readonly IKernel _kernel;

        public TaskFactory(IKernel kernel)
        {
            _kernel = kernel;
        }


        public ITask GetTaskInstance<T>() where T : ITask
        {
            return _kernel.Get<IQueueProcessingTask>();
        }
    }
}