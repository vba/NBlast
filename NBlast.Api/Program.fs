
module NBlast.Api.Console

open System
open Microsoft.Owin.Hosting
open Topshelf
open Topshelf.HostConfigurators
open Topshelf.ServiceConfigurators

(*
public class TownCrier
{
    readonly Timer _timer;
    public TownCrier()
    {
        _timer = new Timer(1000) {AutoReset = true};
        _timer.Elapsed += (sender, eventArgs) => Console.WriteLine("It is {0} an all is well", DateTime.Now);
    }
    public void Start() { _timer.Start(); }
    public void Stop() { _timer.Stop(); }
}
public class Program
{
    public static void Main()
    {
        HostFactory.Run(x =>                                 //1
        {
            x.Service<TownCrier>(s =>                        //2
            {
               s.ConstructUsing(name=> new TownCrier());     //3
               s.WhenStarted(tc => tc.Start());              //4
               s.WhenStopped(tc => tc.Stop());               //5
            });
            x.RunAsLocalSystem();                            //6

            x.SetDescription("Sample Topshelf Host");        //7
            x.SetDisplayName("Stuff");                       //8
            x.SetServiceName("stuff");                       //9
        });                                                  //10
    }
}
*)
type BackgroundJob() =
    let _url = "http://+:8080"
    let _context = lazy(WebApp.Start<WebApiStarter>(_url))
    static let logger = NLog.LogManager.GetCurrentClassLogger()

    do
        "Job initialization finished" |> logger.Debug

    interface ServiceControl with
        member this.Start hc = _context.Value |> ignore; true
        member this.Stop hc  = _context.Value.Dispose(); true
        

//let logger = NLog.LogManager.GetCurrentClassLogger()

[<EntryPoint>]
let main args =

    let service (conf : HostConfigurator) (fac : (unit -> 'a)) =
        let service' = conf.Service : Func<_> -> HostConfigurator
        service' (new Func<_>(fac)) |> ignore
        
    HostFactory.Run(
        fun conf -> 
            conf.Service<_>(new Func<_>(fun sv -> new BackgroundJob())) |> ignore
            conf.SetDisplayName("NBlast Web Api hoster")
            conf.SetServiceName("NBlast.WebApi.Hoster")
            conf.RunAsLocalService() |> ignore

    ) |> ignore
    0