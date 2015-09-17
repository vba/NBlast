using System;
using System.Collections.Concurrent;
using Microsoft.Owin.Hosting;
using Serilog;

namespace NBlast.Rest
{
    public class RestHoster
    {
        private static readonly ILogger Logger = Log.Logger.ForContext<RestHoster>();
        private static readonly ConcurrentQueue<IDisposable>  appsQueue = new ConcurrentQueue<IDisposable>();


        public void Start()
        {
            var baseAddress = "http://+:9090";
            appsQueue.Enqueue(WebApp.Start<Startup>(baseAddress));
            Logger.Information("Started");
        }

        public void Stop()
        {
            IDisposable extracted;
            appsQueue.TryDequeue(out extracted);
            extracted?.Dispose();
            Logger.Information("Stopped");
        }
    }
}