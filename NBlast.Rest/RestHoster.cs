using System;
using System.Collections.Concurrent;
using Microsoft.Owin.Hosting;
using NBlast.Rest.Configuration;
using Serilog;

namespace NBlast.Rest
{
    public class RestHoster
    {
        private static readonly ILogger Logger = Log.Logger.ForContext<RestHoster>();
        private static readonly ConcurrentQueue<IDisposable> AppsQueue = new ConcurrentQueue<IDisposable>();
        private static readonly IConfigReader ConfigReader = new ConfigReader();

        public void Start()
        {
            var baseAddress = ConfigReader.Read("NBlast.api.url");
            AppsQueue.Enqueue(WebApp.Start<Startup>(baseAddress));
            Logger.Information("Started");
        }

        public void Stop()
        {
            IDisposable extracted;
            AppsQueue.TryDequeue(out extracted);
            extracted?.Dispose();
            Logger.Information("Stopped");
        }
    }
}