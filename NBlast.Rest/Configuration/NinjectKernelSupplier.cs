using NBlast.Rest.Async;
using NBlast.Rest.Index;
using NBlast.Rest.Model.Read;
using NBlast.Rest.Model.Write;
using NBlast.Rest.Services.Read;
using NBlast.Rest.Services.Write;
using Read = NBlast.Rest.Index.Read;
using Write = NBlast.Rest.Index.Write;
using Ninject;
using Ninject.Syntax;

namespace NBlast.Rest.Configuration
{
    public class NinjectKernelSupplier
    {
        private const string ReadConfigName              = "Read.Config";
        private const string WriteConfigName             = "Write.Config";
        private const string NblastIndexingDirectoryPath = "NBlast.indexing.directory_path";

        public static IKernel Supply()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IConfigReader>().To<ConfigReader>();

            var configReader = kernel.Get<IConfigReader>();

            ConfigureDirectoryProviders(kernel, configReader);
            kernel.Bind<ILogHitMapperProvider>().ToConstant(new LogHitMapperProvider());
            kernel.Bind<ILogEntryMapperProvider>().ToConstant(new LogEntryMapperProvider());
            kernel.Bind<IIndexingQueueKeeper>().ToConstant(new IndexingQueueKeeper());
            kernel.Bind<IQueueProcessingTask>().To<QueueProcessingTask>();

            ConfigureLuceneDataProviders(kernel);
            ConfigureSearchServices(kernel);

            return kernel;
        }

        private static void ConfigureSearchServices(IBindingRoot kernel)
        {
            kernel.Bind<IStandardSearchService>()
                .ToMethod(x => new StandardSearchService(x.Kernel.Get<ILogHitMapperProvider>(),
                                                         x.Kernel.Get<ILuceneDataProvider>(ReadConfigName)));

            kernel.Bind<ILogEntryIndexationService>()
                .ToMethod(x => new LogEntryIndexationService(x.Kernel.Get<ILuceneDataProvider>(WriteConfigName),
                                                             x.Kernel.Get<ILogEntryMapperProvider>()));
        }

        private static void ConfigureLuceneDataProviders(IBindingRoot kernel)
        {
            kernel.Bind<ILuceneDataProvider>()
                .ToMethod(x => new LuceneDataProvider(x.Kernel.Get<IDirectoryProvider>(ReadConfigName)))
                .Named(ReadConfigName);

            kernel.Bind<ILuceneDataProvider>()
                .ToMethod(x => new LuceneDataProvider(x.Kernel.Get<IDirectoryProvider>(WriteConfigName)))
                .Named(WriteConfigName);
        }

        private static void ConfigureDirectoryProviders(IBindingRoot kernel, IConfigReader configReader)
        {
            kernel.Bind<IDirectoryProvider>()
                .To<Read.FileSystemDirectoryProvider>()
                .Named(ReadConfigName)
                .WithConstructorArgument(configReader.Read(NblastIndexingDirectoryPath));

            kernel.Bind<IDirectoryProvider>()
                .To<Write.FileSystemDirectoryProvider>()
                .Named(WriteConfigName)
                .WithConstructorArgument(configReader.Read(NblastIndexingDirectoryPath));
        }
    }
}