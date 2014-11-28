namespace NBlast.Api

open Owin
open System.Web.Http
open System.Linq
open Microsoft.Practices.Unity
open System.Web.Http.Dependencies

type RouteConfig = {
    id : RouteParameter
}

type WebApiStarter() =

    static let logger = NLog.LogManager.GetCurrentClassLogger()

    member me.Configuration (appBuilder: IAppBuilder): unit =

        logger.Debug("Start self contained WebApi configuration")

        let config = new HttpConfiguration()
        
        config.MapHttpAttributeRoutes()

        config.Routes.MapHttpRoute("DefaultApi",
                                   "api/{controller}/{id}",
                                   {id = RouteParameter.Optional}) |> ignore
        
        let unityResolver = UnityConfig.Configure()
        let appXmlType = 
            config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(
                fun t -> t.MediaType = "application/xml"
        )
        config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType) |> ignore
        config.DependencyResolver <- unityResolver //:> IDependencyResolver

        SchedulerConfig.Configure(unityResolver.Container)

        appBuilder.UseWebApi(config) |> ignore