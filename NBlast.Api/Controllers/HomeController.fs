namespace NBlast.Api

open System.Web.Http

type HomeController() =
    inherit ApiController()

    // GET api/values 
    member this.Get() = 
        ["value1"; "value2"] |> List.toSeq

    // GET api/values/5
    member this.Get(id: int) = 
        "value"