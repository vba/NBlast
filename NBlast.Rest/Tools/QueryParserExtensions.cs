using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using static Lucene.Net.QueryParsers.QueryParser;

namespace NBlast.Rest.Tools
{
    public static class QueryParserExtensions
    {
        public static Query ToLuceneQuery(this string searchQuery, QueryParser parser)
        {
            try
            {
                return parser.Parse(searchQuery?.Trim());
            }
            catch (ParseException)
            {
                return parser.Parse(Escape(searchQuery?.Trim()));
            }
        }
    }
}