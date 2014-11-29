
open FSharp.Collections.ParallelSeq
open Ploeh.AutoFixture
open System.Net
open Microsoft.FSharp.Control.WebExtensions
open RestSharp
open NBlast.Storage.Core

let configReader = new ConfigReader() :> IConfigReader
let restClient = new RestClient(configReader.Read("NBlast.index_endpoint"));

let index (url: string) = 
    let request =  new RestRequest("indexer/index", Method.POST)
    //request.AddParameter("")
    let response = restClient.Execute(request)
    response.StatusCode

[<EntryPoint>]
let main argv = 

    0

