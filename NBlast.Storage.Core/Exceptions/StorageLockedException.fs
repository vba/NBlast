namespace NBlast.Storage.Core.Exceptions

open System.IO


type StorageLockedException =
    inherit IOException
    new (path) = { inherit IOException(sprintf "Index directory is already locked in %s" path) }