using LanguageExt;
using Lucene.Net.Store;
using Moq;
using NBlast.Rest.Index;
using NBlast.Rest.Tools;
using NBlast.Rest.Model.Converters;
using NBlast.Rest.Model.Write;
using NBlast.Rest.Services.Write;
using static LanguageExt.Prelude;
using static Moq.MockBehavior;

namespace NBlast.Rest.Tests.Services.Write
{
    public class LogEntryIndexationServiceTest
    {
        private ILogEntryIndexationService MakeSut(IDirectoryProvider directoryProvider = null,
                                                   IDocumentConverter<LogEntry> logEntryDocumentConverter = null)
        {
            return new LogEntryIndexationService(new FakeDirectoryProvider(),
                                                 logEntryDocumentConverter.ToMonad()
                                                     .If(x => x == null)
                                                     .With(_ => new Mock<IDocumentConverter<LogEntry>>(Strict))
                                                     .Value.Object);
        }

        private class FakeDirectoryProvider: IDirectoryProvider
        {

            public Directory Provide()
            {
                return TryProvide().Some(x => x).None(() => new RAMDirectory());
            }

            public Option<Directory> TryProvide()
            {
                return Some(new RAMDirectory() as Directory);
            }
        }
    }
}