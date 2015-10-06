using System;
using System.Collections.Immutable;
using System.Linq;
using NBlast.Rest.Index;
using NBlast.Rest.Model.Read;

namespace NBlast.Rest.Services.Read
{
    public class StandardSearchService : IStandardSearchService
    {
        public LogHits SearchContent (string query, int skip = 0, int take = 20)
        {
            return null;
        }
        
    }
}