namespace NBlast.Storage.Core.Index

open Newtonsoft.Json

type LogField = 
    | Id
    | Sender
    | Content
    | Error
    | Message
    | Logger
    | Level
    | CreatedAt
    with 
        member me.GetName() = 
            match me with
            | Id        -> "id"
            | Sender    -> "sender"
            | Error     -> "error"
            | Message   -> "message"
            | Logger    -> "logger"
            | Level     -> "level"
            | Content   -> "content"
            | CreatedAt -> "createdAt"

        member me.QueryWith query = me.GetName() + ": "+ query
        
        static member Names = 
            [| LogField.Sender.GetName();
               LogField.Id.GetName();
               LogField.Content.GetName();
               LogField.Message.GetName();
               LogField.Logger.GetName();
               LogField.Level.GetName();
               LogField.CreatedAt.GetName();
               LogField.Error.GetName(); |]

type LogDocumentHit = 
    { [<field: JsonProperty("sender")>]    Sender   : string
      [<field: JsonProperty("id")>]        Id       : string
      [<field: JsonProperty("error")>]     Error    : string
      [<field: JsonProperty("message")>]   Message  : string
      [<field: JsonProperty("logger")>]    Logger   : string
      [<field: JsonProperty("level")>]     Level    : string
      [<field: JsonProperty("boost")>]     Boost    : float32
      [<field: JsonProperty("createdAt")>] CreatedAt: System.DateTime
      [<field: JsonProperty("score")>]     Score    : float32 } 

type LogDocumentHits = 
    { [<field: JsonProperty("hits")>]          Hits          : List<LogDocumentHit>
      [<field: JsonProperty("total")>]         Total         : int32
      [<field: JsonProperty("queryDuration")>] QueryDuration : int64 }