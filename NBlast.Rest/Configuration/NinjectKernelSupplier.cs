using NBlast.Rest.Index;
using NBlast.Rest.Model.Read;
using NBlast.Rest.Model.Write;
using NBlast.Rest.Services.Read;
using Read = NBlast.Rest.Index.Read;
using Write = NBlast.Rest.Index.Write;
using Ninject;

namespace NBlast.Rest.Configuration
{
    public class NinjectKernelSupplier
    {
        public const string ReadDirectoryProviderName   = "Read.DirectoryProvider";
        public const string WriteDirectoryProviderName  = "Write.DirectoryProvider";
        private const string NblastIndexingDirectoryPath = "NBlast.indexing.directory_path";

        public static IKernel Supply()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IConfigReader>().To<ConfigReader>();

            var configReader = kernel.Get<IConfigReader>();

            ConfigureDirectoryProviders(kernel, configReader);
            kernel.Bind<ILogHitMapperProvider>().ToConstant(new LogHitMapperProvider());
            kernel.Bind<ILogEntryMapperProvider>().ToConstant(new LogEntryMapperProvider());

            kernel.Bind<IStandardSearchService>().To<StandardSearchService>();

            return kernel;
        }

        private static void ConfigureDirectoryProviders(StandardKernel kernel, IConfigReader configReader)
        {
            kernel.Bind<IDirectoryProvider>()
                .To<Read.FileSystemDirectoryProvider>()
                .Named(ReadDirectoryProviderName)
                .WithConstructorArgument(configReader.Read(NblastIndexingDirectoryPath));

            kernel.Bind<IDirectoryProvider>()
                .To<Write.FileSystemDirectoryProvider>()
                .Named(WriteDirectoryProviderName)
                .WithConstructorArgument(configReader.Read(NblastIndexingDirectoryPath));
        }
    }
}