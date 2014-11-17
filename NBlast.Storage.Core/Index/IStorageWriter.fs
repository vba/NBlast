namespace NBlast.Storage.Core.Index

type IStorageWriter = interface
    abstract member InsertOne : IStorageDocument -> unit
end