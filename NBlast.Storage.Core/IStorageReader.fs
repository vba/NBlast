namespace NBlast.Storage.Core

type IStorageReader = interface
    abstract member Search : string -> string -> int option -> int option -> LogDocumentHit list
    end
