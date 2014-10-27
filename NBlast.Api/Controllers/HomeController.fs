namespace NBlast.Api

open System.Web.Http

type HomeController() =
    inherit ApiController()

    static let logger = NLog.LogManager.GetCurrentClassLogger()

    // GET api/values 
    member this.Get() =
        let response = ["value1"; "value2"] |> List.toSeq
        logger.Debug(sprintf "Response %A" response)
        response

    // GET api/values/5
    member this.Get(id: int) = 
        "value"