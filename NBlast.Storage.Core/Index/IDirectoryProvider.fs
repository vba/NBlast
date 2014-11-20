namespace NBlast.Storage.Core.Index

open Lucene.Net.Store

type IDirectoryProvider = interface
    abstract member Provide: unit -> Directory
    end