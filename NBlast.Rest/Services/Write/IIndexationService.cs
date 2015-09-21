using System.Collections.Generic;
using LanguageExt;

namespace NBlast.Rest.Services.Write
{
    public interface IIndexationService<in T>
    {
        Unit IndexOne(T entry);

        Unit IndexMany(IReadOnlyList<T> entries);
    }
}