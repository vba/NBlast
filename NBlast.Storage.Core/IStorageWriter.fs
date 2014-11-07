namespace NBlast.Storage.Core

type IStorageWriter = interface
    abstract member InsertOne : IStorageDocument -> unit
end