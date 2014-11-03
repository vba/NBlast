namespace NBlast.Storage

open Lucene.Net.Documents
open System

type StorageType = 
    | Analysed = 1 
    | Stored = 2
    | Both = 3 

type Field<'T> = 
    { Value : 'T
      Name: string
      StorageType: StorageType } 
    member this.ToLuceneField() =
        let value = match this.Value with
//            | :? DateTime -> 
//                DateTools.TimeToString(this.Value :?> DateTime)
                    | _ -> this.Value.ToString()
        0


type LogDocument = 
    { message  : Field<string>
      logger   : Field<string>
      level    : Field<string>
      error    : Field<string>
      createdAt: Field<DateTime> }

    member this.ToLuceneDocument() = 
        
        0

