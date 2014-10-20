namespace NBlast.Api

open Owin
open System.Web.Http

type RouteConfig = {
    id : RouteParameter
}

type WebApiStarter() =
    member this.Configuration (appBuilder: IAppBuilder): unit =
        let config = new HttpConfiguration()
        config.Routes.MapHttpRoute("DefaultApi", 
                                   "api/{controller}/{id}", 
                                   {id = RouteParameter.Optional}) |> ignore
        appBuilder.UseWebApi(config) |> ignore
        appBuilder.UseNancy() |> ignore
