namespace NBlast.Storage.Core.Index

type IStorageReader = interface
    abstract member SearchByField : string -> int option -> int option -> LogDocumentHits
    abstract member FindAll: int option -> int option -> LogDocumentHits
    abstract member CountAll: unit -> int
    end
