using System;
using System.Collections.Generic;
using System.Web.Http;
using NBlast.Rest.Configuration;
using NBlast.Rest.Services.Read;
using Ninject.Infrastructure.Language;

namespace NBlast.Rest.Controllers.Api
{
    [RoutePrefix("api/values")]
    public class ValuesController: ApiController
    {
        private readonly IStandardSearchService _searchService;

        public ValuesController(IStandardSearchService searchService)
        {
            if (searchService == null) throw new ArgumentNullException(nameof(searchService));
            _searchService = searchService;
        }

        // GET api/values 
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" }.ToEnumerable();
        }

        // GET api/values/5 
        public string Get(int id)
        {
            return "value";
        }
    }
}