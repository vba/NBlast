namespace NBlast.Api.Models

open Newtonsoft.Json

type QueueModel = {
    [<field: JsonProperty("logs")>] Logs    : LogModel seq
    [<field: JsonProperty("total")>] Total  : int32
}

