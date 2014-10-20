
module NBlast.Api.Console

open System
open Microsoft.Owin.Hosting

[<EntryPoint>]
let main args =
    let url = "http://+:8080"
    use starter = WebApp.Start<WebApiStarter>(url)
    Console.WriteLine("Running on {0}", url)
    Console.WriteLine("Press enter to exit")
    Console.ReadLine() |> ignore
    0