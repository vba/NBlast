namespace NBlast.Storage

open Lucene.Net.Documents
open Lucene.Net
open System

type StorageType = 
    | Analysed = 1 
    | Stored = 2
    | Both = 3
    
module FieldUtils = 
    let toLuceneField name value (storageType: StorageType) =
        let store = 
            match storageType with
                | StorageType.Analysed -> (Documents.Field.Store.NO, Documents.Field.Index.ANALYZED_NO_NORMS)
                | StorageType.Stored -> (Documents.Field.Store.YES, Documents.Field.Index.NO)
                | _ -> (Documents.Field.Store.YES, Documents.Field.Index.ANALYZED_NO_NORMS)
        new Lucene.Net.Documents.Field(name, value, fst store, snd store)
 

type Field = 
    { Value      : string
      Name       : string
      StorageType: StorageType }

    member this.ToLuceneField() = 
        FieldUtils.toLuceneField this.Name this.Value this.StorageType
        
type DateTimeField =
    { Value       : DateTime
      Name        : String
      StorageType : StorageType }

    member this.ToLuceneField() = 
        let value = DateTools.DateToString(this.Value, DateTools.Resolution.SECOND)
        FieldUtils.toLuceneField this.Name value this.StorageType


type LogDocument = 
    { message  : Field
      logger   : Field
      level    : Field
      error    : Field
      createdAt: DateTime }

    member this.ToLuceneDocument() = 
        
        0

