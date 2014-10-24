namespace NBlast.Api

open Owin
open System.Web.Http
open System.Linq

type RouteConfig = {
    id : RouteParameter
}

type WebApiStarter() =
    member this.Configuration (appBuilder: IAppBuilder): unit =
        let config = new HttpConfiguration()

        config.Routes.MapHttpRoute("DefaultApi",
                                   "api/{controller}/{id}",
                                   {id = RouteParameter.Optional}) |> ignore

        let appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(
                            fun t -> t.MediaType = "application/xml"
        )
        config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType) |> ignore

        appBuilder.UseWebApi(config) |> ignore
