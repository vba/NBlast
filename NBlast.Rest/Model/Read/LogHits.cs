using System;
using System.Collections.Generic;

namespace NBlast.Rest.Model.Read
{
    public class LogHits
    {
        public long Duration { get; }
        public int Total { get; }
        public IReadOnlyList<LogHit> Hits { get; }

        public LogHits(long duration, int total, IReadOnlyList<LogHit> hits)
        {
            if (hits == null) throw new ArgumentNullException(nameof(hits));
            Duration = duration;
            Total    = total;
            Hits     = hits;
            
//            new ClassMap
        }
    }
}