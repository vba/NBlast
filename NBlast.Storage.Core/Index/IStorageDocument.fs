namespace NBlast.Storage.Core.Index

open Lucene.Net.Documents

type IStorageDocument = interface
    abstract member ToLuceneDocument : unit -> Document
end