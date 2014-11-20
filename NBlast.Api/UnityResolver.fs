namespace NBlast.Api

open System
open System.Web.Http.Dependencies
open Microsoft.Practices.Unity

type UnityResolver(container: IUnityContainer) =

    interface IDependencyResolver with 
        member me.GetService (serviceType: Type) =
            try
                container.Resolve(serviceType)
            with
                | :? ResolutionFailedException -> None :> obj

        member me.GetServices (serviceType: Type) = 
            try
                container.ResolveAll(serviceType)
            with
                | :? ResolutionFailedException -> Seq.empty

        member me.BeginScope() = 
            let child = container.CreateChildContainer()
            new UnityResolver(child) :> IDependencyScope

        member me.Dispose() = 
            container.Dispose()