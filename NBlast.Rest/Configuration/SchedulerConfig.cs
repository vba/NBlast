using System;
using FluentScheduler;
using FluentScheduler.Model;
using LanguageExt;
using Ninject;
using Serilog;
using static LanguageExt.Prelude;

namespace NBlast.Rest.Configuration
{
    public class SchedulerConfig
    {
        private static readonly ILogger Logger = Log.Logger.ForContext<SchedulerConfig>();
        private static readonly string RunEveryMinutes = "NBlast.indexing.scheduler.run_every_minutes";

        private static void OnTaskException(TaskExceptionInformation sender, UnhandledExceptionEventArgs args)
        {
            var ex = args.ExceptionObject as Exception;
            if (ex == null) return;
            Logger.Fatal(ex, ex.Message);
        }

        public static Unit Configure(IKernel kernel)
        {
            Logger.Debug("Start to configure task scheduler");

            var configReader = kernel.Get<IConfigReader>();
            var minutes      = configReader.ReadAsInt(RunEveryMinutes);
            var registry     = new SchedulerRegistry();
            
            Logger.Debug($"Schedule task runner for every {minutes} minute(s)");

            TaskManager.TaskFactory = new TaskFactory(kernel);
            TaskManager.UnobservedTaskException += OnTaskException;
            TaskManager.Initialize(registry);
            return unit;
        }
    }
}