namespace NBlast.Api.Models

open Newtonsoft.Json
open System
open System.ComponentModel.DataAnnotations


type LogModel () =
    [<field: JsonIgnore>]
    let mutable error: string option = None
    
    [<field: JsonIgnore>]
    let mutable createdAt: DateTime option = None

    [<Required>]
    [<field: JsonProperty("sender")>]
    member val Sender: string = null with get, set
    
    [<Required>]
    [<field: JsonProperty("message")>]
    member val Message: string = null with get, set
    
    [<Required>]
    [<field: JsonProperty("logger")>]
    member val Logger: string = null with get, set
    
    [<Required>]
    [<field: JsonProperty("level")>]
    member val Level: string = null with get, set
    
    [<JsonIgnore>]
    member me.Error 
        with set (value: string) = 
          error <- if(String.IsNullOrEmpty(value)) then None else Some value
    
    [<JsonIgnore>]
    member me.CreatedAt 
        with set (value: Nullable<DateTime>) = 
          createdAt <- if(value.HasValue) then Some value.Value else None

    [<JsonProperty("error")>]
    member me.ErrorOp with get() = error

    [<JsonProperty("createdAt")>]
    member me.CreatedAtOp with get() = createdAt
