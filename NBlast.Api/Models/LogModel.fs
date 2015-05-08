namespace NBlast.Api.Models

open Newtonsoft.Json
open System
open System.Net
open System.Web.Http.ModelBinding
open System.ComponentModel.DataAnnotations
open System.ComponentModel
open System.Collections.Specialized


//[<ModelBinder(typeof<LogModelBinder>)>]
type LogModel () =

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

    [<field: JsonProperty("error")>]
    member val Error: string = null with get, set 

    [<field: JsonProperty("createdAt")>]
    member val CreatedAt: Nullable<DateTime> = Unchecked.defaultof<_> with get, set 

    [<JsonIgnore>]
    member me.ErrorOp with get() = if(String.IsNullOrEmpty(me.Error)) then None else Some me.Error

    [<JsonIgnore>]
    member me.CreatedAtOp with get() = if (me.CreatedAt.HasValue) then Some me.CreatedAt.Value else None

    override me.ToString() = 
        sprintf "{sender=%s, logger=%s, level=%s, message=%s, createdAt=%s}" 
                    me.Sender 
                    me.Logger
                    me.Level
                    me.Message
                    (if me.CreatedAt.HasValue then me.CreatedAt.Value.ToString() else "<NULL>")

and LogModelBinder() =
    member private me.TryParseAsBody(value:string) = 
        value.Split('&') |> Seq.map (fun x -> 
            match x.Split('=') |> Seq.toList with
            | (key :: value :: _) -> (key.Trim([|'?'; ' '|]), WebUtility.UrlDecode(value.Trim())) |> Some
            | _ -> None
        ) |> Seq.fold (fun (acc:NameValueCollection) elem -> 
            match elem with
            | Some(kv) -> acc.Add(fst kv, snd kv); acc
            | _ -> acc
        ) (new NameValueCollection())

    member private me.TryParseAsJson(value:string) = ""
    
    member private me.BindFromStringValue value (context: ModelBindingContext) = 
        true

    member private me.BindFromRawValue (value:obj) (context: ModelBindingContext) = 
        match value with
        | :? String -> me.BindFromStringValue (value |> string) context
        | _ -> 
            context.ModelState.AddModelError(context.ModelName, "Wrong value type")
            false

    member private me.BindFromContext(context: ModelBindingContext) =
        let value = context.ValueProvider.GetValue(context.ModelName)
        
        if (value = null) then false
        else me.BindFromRawValue value.RawValue context

    interface IModelBinder with
        member me.BindModel(actionContext, bindingContext) = 
            match bindingContext.ModelType = typeof<LogModel> with 
            | true -> me.BindFromContext(bindingContext)
            | _ -> false
            // TODO follow implementation with http://www.asp.net/web-api/overview/formats-and-model-binding/parameter-binding-in-aspnet-web-api
