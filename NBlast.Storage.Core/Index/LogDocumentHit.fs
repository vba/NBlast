namespace NBlast.Storage.Core.Index

open Newtonsoft.Json

type LogDocumentHit = 
    { [<field: JsonProperty("sender")>]    Sender    : string
      [<field: JsonProperty("id")>]        Id        : string
      [<field: JsonProperty("error")>]     Error     : string
      [<field: JsonProperty("message")>]   Message   : string
      [<field: JsonProperty("logger")>]    Logger    : string
      [<field: JsonProperty("level")>]     Level     : string
      [<field: JsonProperty("boost")>]     Boost     : float32
      [<field: JsonProperty("createdAt")>] CreatedAt : System.DateTime
      [<field: JsonProperty("score")>]     Score     : float32 } 

type LogDocumentHits = 
    { [<field: JsonProperty("hits")>]          Hits          : List<LogDocumentHit>
      [<field: JsonProperty("total")>]         Total         : int32
      [<field: JsonProperty("queryDuration")>] QueryDuration : int64 }

    static member GetEmpty() = {Hits = []; Total = 0; QueryDuration = 0L}