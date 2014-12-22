namespace NBlast.Fixtures.Console

open System.Diagnostics.CodeAnalysis
open FSharp.Collections.ParallelSeq
open Ploeh.AutoFixture
open System.Net
open Microsoft.FSharp.Control.WebExtensions
open RestSharp
open NBlast.Storage.Core
open NBlast.Api.Models

[<ExcludeFromCodeCoverage>]
module App =
    let configReader = new ConfigReader() :> IConfigReader
    let fixturesLimit = configReader.ReadAsInt("NBlast.fixtures_limit")
    let restClient = new RestClient(configReader.Read("NBlast.index_endpoint"))
    let fixture = new Fixture()

    let index (log: LogModel) = 
        let request =  new RestRequest("indexer/index", Method.POST)
        request.AddParameter("sender", log.Sender) |> ignore
        request.AddParameter("message", log.Message) |> ignore
        request.AddParameter("level", log.Level) |> ignore
        request.AddParameter("logger", log.Logger) |> ignore
        request.AddParameter("error", log.Error) |> ignore
        //request.AddParameter("createdAt", log.CreatedAt) |> ignore
        
        let response = restClient.Execute(request)
        response.StatusCode |> printfn "Response status code is %A"
        response.StatusCode

    let generate () =
        let levels = ["fatal"; "error"; "warn"; "info"; "debug"; "trace"]
        fixture.CreateMany<LogModel>(fixturesLimit) |> PSeq.map (fun x -> 
            x.Level <- levels.[(new System.Random()).Next(levels.Length)]
            x
        )

    [<EntryPoint>]
    let main argv = 
        let processed = 
            generate() 
                |> PSeq.map (fun log -> log |> index)
                |> PSeq.filter (fun status -> status = HttpStatusCode.OK) 
                |> PSeq.toList
        processed.Length |> printfn "%d records were indexed successfully, Press any key for exit"
        System.Console.ReadLine() |> ignore
        0

