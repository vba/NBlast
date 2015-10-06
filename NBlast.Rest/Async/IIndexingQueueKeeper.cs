using NBlast.Rest.Model.Dto;

namespace NBlast.Rest.Async
{
    public interface IIndexingQueueKeeper: IQueueKeeper<LogEvent>
    {
    }
}