
module NBlast.Api.Console

open System
open Microsoft.Owin.Hosting
open Topshelf
open Topshelf.HostConfigurators

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
    type BackgroundJob()=
        let _url = "http://+:8080"
        let _context = lazy(WebApp.Start<WebApiStarter>(_url))
        do
            printfn "BackgroundJob initialization finished"

        member this.Start() = _context.Value |> ignore; true
        member this.Stop()  = _context.Value.Dispose(); true
        
    
[<EntryPoint>]
let main args =

    let url = "http://+:8080"

    let serviceControl (start : HostControl -> bool) (stop : HostControl -> bool) =
        { new ServiceControl with
            member x.Start hc =
                start hc
            member x.Stop hc =
                stop hc }

    let runService () =
        BackgroundJob()
        |> fun job -> serviceControl (fun hc -> job.Start()) (fun hc -> job.Stop())

    let service (conf : HostConfigurator) (fac : (unit -> 'a)) =
        let service' = conf.Service : Func<_> -> HostConfigurator
        service' (new Func<_>(fac)) |> ignore

    let serviceFunc = fun conf ->
        runService |> service conf

    let configureTopShelf f =
        HostFactory.Run(new Action<_>(f)) |> ignore

    configureTopShelf <| fun conf -> (runService |> service conf)
    (*
    use starter = WebApp.Start<WebApiStarter>(url)
    Console.WriteLine("Running on {0}", url)
    Console.WriteLine("Press enter to exit")
    Console.ReadLine() |> ignore
    *)
    0