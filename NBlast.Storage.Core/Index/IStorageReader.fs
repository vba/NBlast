namespace NBlast.Storage.Core.Index

type IStorageReader = interface
    abstract member SearchByField : SearchQuery -> LogDocumentHits
    abstract member FindAll: int option -> int option -> LogDocumentHits
    abstract member CountAll: unit -> int
    abstract member GroupWith: LogField -> SimpleSenderFacets
    end
