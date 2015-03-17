namespace NBlast.Storage.Core.Index

open Lucene.Net.Store

type IDirectoryProvider = interface
    abstract member TryProvide: unit -> Directory option
    abstract member Provide: unit -> Directory
    end