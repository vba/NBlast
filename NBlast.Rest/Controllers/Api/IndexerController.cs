using System;
using System.Web.Http;
using NBlast.Rest.Async;

namespace NBlast.Rest.Controllers.Api
{
    [RoutePrefix("api/indexer")]
    public class IndexerController: ApiController
    {
        private readonly IIndexingQueueKeeper _indexingQueueKeeper;

        public IndexerController(IIndexingQueueKeeper indexingQueueKeeper)
        {
            if (indexingQueueKeeper == null) throw new ArgumentNullException(nameof(indexingQueueKeeper));
            _indexingQueueKeeper = indexingQueueKeeper;
        }


    }
}