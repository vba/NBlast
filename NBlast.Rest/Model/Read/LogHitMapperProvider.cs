using Lucene.Net.Linq.Fluent;
using Lucene.Net.Linq.Mapping;
using Lucene.Net.Util;

namespace NBlast.Rest.Model.Read
{
    public class LogHitMapperProvider : ILogHitMapperProvider
    {
        private readonly ClassMap<LogHit> _classMap = new ClassMap<LogHit>(Version.LUCENE_30);

        public LogHitMapperProvider()
        {
            _classMap.Key(x => x.Id);
            _classMap.Score(x => x.Score);
            _classMap.DocumentBoost(x => x.Boost);
        }

        public IDocumentMapper<LogHit> Provide()
        {
            return _classMap.ToDocumentMapper();
        }
    }
}