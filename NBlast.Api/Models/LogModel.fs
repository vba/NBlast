namespace NBlast.Api.Models

open System
open System.ComponentModel.DataAnnotations


type LogModel () =
    let mutable error: string option = None
    let mutable createdAt: DateTime option = None

    [<Required>]
    member val Sender: string = null with get, set
    [<Required>]
    member val Message: string = null with get, set
    [<Required>]
    member val Logger: string = null with get, set
    [<Required>]
    member val Level: string = null with get, set
    
    member me.Error 
        with set (value: string) = 
          error <- if(String.IsNullOrEmpty(value)) then None else Some value

    member me.CreatedAt 
        with set (value: Nullable<DateTime>) = 
          createdAt <- if(value.HasValue) then Some value.Value else None

    member me.ErrorOp with get() = error
    member me.CreatedAtOp with get() = createdAt
