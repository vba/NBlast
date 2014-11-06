namespace NBlast.Storage

open NBlast.Storage.Core
open Lucene.Net.Documents
open Lucene.Net
open System

type StorageType = 
    | Analysed = 1 
    | Stored   = 2
    | Both     = 3
    
module FieldUtils = 
    let toLuceneField name value (storageType: StorageType) =
        let store = 
            match storageType with
                | StorageType.Analysed -> (Documents.Field.Store.NO, Documents.Field.Index.ANALYZED_NO_NORMS)
                | StorageType.Stored -> (Documents.Field.Store.YES, Documents.Field.Index.NO)
                | _ -> (Documents.Field.Store.YES, Documents.Field.Index.ANALYZED_NO_NORMS)
        new Lucene.Net.Documents.Field(name, value, fst store, snd store) :> IFieldable
 

type IField<'T> =
    abstract member Name: unit -> string with get
    abstract member StorageType: unit -> StorageType with get
    abstract member Value: unit -> 'T with get
    abstract member ToLuceneField: unit -> IFieldable

type TextField(name        : string, 
               value       : string, 
               storageType : StorageType) =

    new (name, value) = 
        TextField(name = name, value = value, storageType = StorageType.Both)
    
    interface IField<string> with
        member this.Name with get() = name
        member this.Value with get() = value
        member this.StorageType with get() = storageType
        member this.ToLuceneField() = FieldUtils.toLuceneField name value storageType

type DateTimeField(name        : string,
                   value       : DateTime,
                   storageType : StorageType) =

    new (name, value) = 
        DateTimeField(name = name, value = value, storageType = StorageType.Stored)

    interface IField<DateTime> with
        member this.Name with get() = name
        member this.Value with get() = value
        member this.StorageType with get() = storageType
        member this.ToLuceneField() = 
            let value = DateTools.DateToString(value, DateTools.Resolution.SECOND)
            FieldUtils.toLuceneField name value storageType

type LogDocument ( message   : TextField,
                   logger    : TextField,
                   level     : TextField,
                   error     : TextField,
                   createdAt : DateTimeField ) =
    
    member this.Message with get() = message :> IField<string>
    member this.Logger with get() = logger :> IField<string>
    member this.Error with get() = error :> IField<string>
    member this.Level with get() = level :> IField<string>
    member this.CreatedAt with get() = createdAt :> IField<DateTime>

    interface IStorageDocument with
        member this.ToLuceneDocument() = 
            let document = new Document() 
            this.Message.ToLuceneField() |> document.Add
            this.Logger.ToLuceneField() |> document.Add
            this.Level.ToLuceneField() |> document.Add
            this.Error.ToLuceneField() |> document.Add
            this.CreatedAt.ToLuceneField() |> document.Add
            document
