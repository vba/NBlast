namespace NBlast.Storage.Core.Index

type IStorageWriter = interface
    abstract member InsertOne : IStorageDocument -> unit
    abstract member InsertMany : seq<IStorageDocument> -> unit
end