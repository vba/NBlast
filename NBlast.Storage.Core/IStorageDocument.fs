namespace NBlast.Storage.Core

open Lucene.Net.Documents

type IStorageDocument = interface
    abstract member ToLuceneDocument : unit -> Document
end