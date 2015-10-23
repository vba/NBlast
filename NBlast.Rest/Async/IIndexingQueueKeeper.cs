using NBlast.Rest.Model.Write;

namespace NBlast.Rest.Async
{
    public interface IIndexingQueueKeeper: IQueueKeeper<LogEvent>
    {
    }
}