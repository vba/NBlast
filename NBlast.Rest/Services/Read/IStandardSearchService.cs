using NBlast.Rest.Model.Read;

namespace NBlast.Rest.Services.Read
{
    public interface IStandardSearchService
    {
        LogHits SearchContent (string query, int skip = 0, int take = 20);
    }
}