using Ninject;

namespace NBlast.Rest.Configuration
{
    public class NinjectKernelSupplier
    {
         public static IKernel Supply()
         {
            var kernel = new StandardKernel();
            kernel.Bind<IConfigReader>().To<ConfigReader>();

            var configReader = kernel.Get<IConfigReader>();

            return kernel;
         }
    }
}