namespace NBlast.Storage

open NBlast.Storage.Core
open NBlast.Storage.Core.Index
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

type SenderField (value: string)      = inherit TextField(LogField.Sender.GetName(), value)
type LoggerField (value: string)      = inherit TextField(LogField.Logger.GetName(), value)
type ErrorField (value: string)       = inherit TextField(LogField.Error.GetName(), value)
type MessageField (value: string)     = inherit TextField(LogField.Message.GetName(), value)
type LevelField (value: string)       = inherit TextField(LogField.Level.GetName(), value)
type CreatedAtField (value: DateTime) = inherit DateTimeField(LogField.CreatedAt.GetName(), value)
type ContentField (value: string)     = inherit TextField(LogField.Content.GetName(), value, StorageType.Analysed)


type LogDocument ( sender     : string,
                   message    : string,
                   logger     : string,
                   level      : string,
                   ?error     : string,
                   ?createdAt : DateTime ) =

    member me.Sender with get() = new SenderField(sender)  :> IField<string>
    member me.Message with get() = new MessageField(message)  :> IField<string>
    member me.Logger with get() = new LoggerField(logger)  :> IField<string>
    member me.Level with get() = new LevelField(level)  :> IField<string>
    member me.Content with get() = new ContentField(if (error.IsSome) then message + " " + error.Value else message ) :> IField<string>
    member me.CreatedAt with get() = new CreatedAtField(if (createdAt.IsSome) then createdAt.Value else DateTime.Now)  :> IField<DateTime>
    member me.Error with get() =  if (error.IsSome) 
                                    then Some(new ErrorField(error.Value)  :> IField<string>)
                                    else None


    interface IStorageDocument with
        member me.ToLuceneDocument() = 
            let document = new Document()
            me.Sender.ToLuceneField() |> document.Add
            me.Message.ToLuceneField() |> document.Add
            me.Logger.ToLuceneField() |> document.Add
            me.Level.ToLuceneField() |> document.Add
            me.CreatedAt.ToLuceneField() |> document.Add
            me.Content.ToLuceneField() |> document.Add
            
            if (error.IsSome) then
                me.Error.Value.ToLuceneField() |> document.Add

            document