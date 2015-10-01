namespace NBlast.Rest.Services.Write
{
    public interface IConverter<in TIn, out TOut>
    {
        TOut Convert(TIn logModel);
    }
}