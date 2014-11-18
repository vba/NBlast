namespace NBlast.Storage.Core.Index

type DocumentField = {Name : string}

type LogField = 
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
               LogField.Content.GetName();
               LogField.Message.GetName();
               LogField.Logger.GetName();
               LogField.Level.GetName();
               LogField.CreatedAt.GetName();
               LogField.Error.GetName(); |]

type LogDocumentHit = 
    { Sender   : string
      Error    : string
      Message  : string
      Logger   : string
      Level    : string
      Boost    : float32
      CreatedAt: System.DateTime
      Score    : float32 option }

type LogDocumentHits = 
    { Hits          : List<LogDocumentHit>
      Total         : int32
      QueryDuration : int64 }