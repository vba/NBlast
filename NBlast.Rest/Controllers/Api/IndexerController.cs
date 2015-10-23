using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Results;
using NBlast.Rest.Async;
using NBlast.Rest.Model.Write;
using Serilog;

namespace NBlast.Rest.Controllers.Api
{
    [EnableCors("*", "*", "GET")]
    [RoutePrefix("api/indexer")]
    public class IndexerController: ApiController
    {
        private readonly IIndexingQueueKeeper _indexingQueueKeeper;
        private static readonly ILogger Logger = Log.Logger.ForContext<IndexerController>();

        public IndexerController(IIndexingQueueKeeper indexingQueueKeeper)
        {
            if (indexingQueueKeeper == null) throw new ArgumentNullException(nameof(indexingQueueKeeper));
            _indexingQueueKeeper = indexingQueueKeeper;
        }


        [HttpPost]
        [Route("index")]
        public IHttpActionResult Index(LogEvent @event)
        {
            if (!ModelState.IsValid)
            {
                Logger.Debug($"Log model {@event} is INVALID");
                return BadRequest(ModelState);
            }
            _indexingQueueKeeper.Enqueue(@event); // TODO Maybe clone it before
            return Ok(@event);
        }

        [HttpPost]
        [Route("index-events")]
        public IHttpActionResult Index(LogEvents eventsKeeper)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            eventsKeeper.Events.ToList().ForEach(e => _indexingQueueKeeper.Enqueue(e));
            return Ok();
        }

        [HttpGet]
        [Route("queue-count")]
        public int QueueCount() => _indexingQueueKeeper.Count();

        [HttpGet]
        [Route("queue-content/{top}")]
        public IReadOnlyList<LogEvent> QueueContent(int top) =>
            _indexingQueueKeeper.PeekTop(top);
    }
}