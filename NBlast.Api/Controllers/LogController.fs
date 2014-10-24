namespace NBlast.Api

open System.Web.Http

type LogController() = 
    inherit ApiController()

    [<HttpPost>]
    [<Route("receive")>]
    member me.Receive () = 
        "Ok"
