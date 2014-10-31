namespace NBlast.Storage.Core.Exceptions

open System.IO

type StorageUnlockFailedException =
    inherit IOException
    new (path) = { inherit IOException(sprintf "Unable to unlock index directory in %s" path) }