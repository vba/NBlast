namespace NBlast.Storage.Core

open Lucene.Net.Documents

type IStorageDocument = 
    abstract member ToLuceneDocument : unit -> Document