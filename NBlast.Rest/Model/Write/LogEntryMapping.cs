using Lucene.Net.Linq;
using Lucene.Net.Linq.Fluent;
using static Lucene.Net.Util.Version;

namespace NBlast.Rest.Model.Write
{
    public interface IMapping<T>
    {
        ClassMap<T> Create();
    }

    public class LogEntryMapping : IMapping<LogEntry>
    {
        public ClassMap<LogEntry> Create()
        {
            var mapping = new ClassMap<LogEntry>(LUCENE_30);

            

            return mapping;
        }
    }
}