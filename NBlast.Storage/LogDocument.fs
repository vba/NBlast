namespace NBlast.Storage

open NBlast.Storage.Core
open Lucene.Net.Documents
open Lucene.Net
open System

type StorageType = 
    | None                  = 0
    | Analysed              = 1 
    | Stored                = 2
    | StoredAndAnalysed     = 3
    
module FieldUtils = 
    let toLuceneField name value (storageType: StorageType) =
        let store = 
            match storageType with
                | StorageType.None -> (Documents.Field.Store.NO, Documents.Field.Index.NOT_ANALYZED_NO_NORMS)
                | StorageType.Analysed -> (Documents.Field.Store.NO, Documents.Field.Index.ANALYZED_NO_NORMS)
                | StorageType.Stored -> (Documents.Field.Store.YES, Documents.Field.Index.NOT_ANALYZED_NO_NORMS)
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
        TextField(name = name, value = value, storageType = StorageType.Stored)
    
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

type SenderField (value: string) = inherit TextField("sender", value)
type LoggerField (value: string) = inherit TextField("logger", value)
type ErrorField (value: string) = inherit TextField("error", value)
type MessageField (value: string) = inherit TextField("message", value)
type LevelField (value: string) = inherit TextField("level", value)
type CreatedAtField (value: DateTime) = inherit DateTimeField("createdAt", value)


type LogDocument ( sender     : SenderField,
                   message    : MessageField,
                   logger     : LoggerField,
                   level      : LevelField,
                   ?error     : ErrorField,
                   ?createdAt : CreatedAtField ) =
    
    new ( sender    : string,
          message   : string,
          logger    : string,
          level     : string,
          ?error    : string,
          ?createdAt: DateTime) = 
            let sf = new SenderField(sender)
            let mf = new MessageField(message)
            let lf = new LoggerField(logger)
            let lg = new LevelField(level)
            
            match (error, createdAt) with
                | (Some error, None) -> 
                    LogDocument(sf, mf, lf, lg, new ErrorField(error))
                | (Some error, Some createdAt) -> 
                    LogDocument(sf, mf, lf, lg, new ErrorField(error), new CreatedAtField(createdAt))
                | (None, Some createdAt) -> 
                    LogDocument(sf, mf, lf, lg, createdAt = new CreatedAtField(createdAt))
                | _ -> LogDocument(sf, mf, lf, lg)

    interface IStorageDocument with
        member this.ToLuceneDocument() = 
            let document = new Document() 
            (sender :> IField<string>).ToLuceneField() |> document.Add
            (message :> IField<string>).ToLuceneField() |> document.Add
            (logger :> IField<string>).ToLuceneField() |> document.Add
            (level :> IField<string>).ToLuceneField() |> document.Add
            
            if (error.IsSome) then
                (error.Value :> IField<string>).ToLuceneField() |> document.Add
            if (createdAt.IsSome) then
                (createdAt.Value :> IField<DateTime>).ToLuceneField() |> document.Add
            
            let content = 
                if (error.IsSome) 
                    then (message :> IField<_>).Value+" "+(error.Value :> IField<_>).Value
                    else (message :> IField<_>).Value

            ((new TextField("content", content, StorageType.StoredAndAnalysed)) :> IField<string>)
                .ToLuceneField() |> document.Add

            document