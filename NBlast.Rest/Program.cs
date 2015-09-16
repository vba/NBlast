using Serilog;
using Serilog.Enrichers;
using Topshelf;

namespace NBlast.Rest
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<RestHoster>(s =>
                {
                    s.ConstructUsing(n => new RestHoster());
                    s.WhenStarted(hs => hs.Start());
                    s.WhenStopped(hs => hs.Stop());
                });
                
                x.RunAsLocalService();
                x.SetDisplayName("NBlast rest endpoint service");
                x.SetServiceName("NBlast.Rest");
            });
        }

        static Program()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.ColoredConsole()
                .MinimumLevel.Verbose()
                .CreateLogger();
        }
    }
}
