namespace NBlast.Api

open System.Web.Http
open System.Net.Http.Formatting

type LogController() = 
    inherit ApiController()

    [<HttpPost>]
    [<Route("receive")>]
    member me.Receive (formData: FormDataCollection) =
        let values = formData |> Seq.map (fun x -> (x.Key, x.Value)) 
        printfn "values: %A" values
        "Ok"
