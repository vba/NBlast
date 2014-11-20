namespace NBlast.Api

open System
open System.Web.Http.Dependencies
open Microsoft.Practices.Unity

type UnityResolver(container: IUnityContainer) =

    interface IDependencyResolver with 
        member me.GetService (serviceType: Type) =
            container.Resolve(serviceType)
        member me.GetServices (serviceType: Type) = 
            container.ResolveAll(serviceType)
        member me.BeginScope() = 
            let child = container.CreateChildContainer()
            new UnityResolver(child) :> IDependencyScope
        member me.Dispose() = 
            container.Dispose()

(*

public class UnityResolver : IDependencyResolver
{
    protected IUnityContainer container;

    public UnityResolver(IUnityContainer container)
    {
        if (container == null)
        {
            throw new ArgumentNullException("container");
        }
        this.container = container;
    }

    public object GetService(Type serviceType)
    {
        try
        {
            return container.Resolve(serviceType);
        }
        catch (ResolutionFailedException)
        {
            return null;
        }
    }

    public IEnumerable<object> GetServices(Type serviceType)
    {
        try
        {
            return container.ResolveAll(serviceType);
        }
        catch (ResolutionFailedException)
        {
            return new List<object>();
        }
    }

    public IDependencyScope BeginScope()
    {
        var child = container.CreateChildContainer();
        return new UnityResolver(child);
    }

    public void Dispose()
    {
        container.Dispose();
    }
}



public static void Register(HttpConfiguration config)
{
    var container = new UnityContainer();
    container.RegisterType<IProductRepository, ProductRepository>(new HierarchicalLifetimeManager());
    config.DependencyResolver = new UnityResolver(container);

    // Other Web API configuration not shown.
}
*)