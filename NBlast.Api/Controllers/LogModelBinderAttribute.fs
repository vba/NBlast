namespace NBlast.Api.Controllers

open System.Web.Http
open System.Web.Http.Filters
open System.Web.Http.Controllers
open System.Net.Http.Formatting
open NBlast.Api.Models
open System.Threading.Tasks
open System
open Newtonsoft.Json


type LogModelParameterBinding(target: string, source: string, descriptor) =
    inherit HttpParameterBinding(descriptor)
    
    let broken = new LogModel()

    override me.ExecuteBindingAsync(metadataProvider, actionContext, cancellationToken) =
        let source = (actionContext.ActionArguments.[source] :?> FormDataCollection).ReadAsNameValueCollection()
        actionContext.ActionArguments.[target] <- 
            match source.Count with
                | 0 -> broken
                | 1 -> source.Get(0) |> me.TryParseAsJson
                | _ -> 
                    new LogModel(Message   = source.["message"],
                                 Sender    = source.["sender"],
                                 Level     = source.["level"],
                                 Error     = source.["error"],
                                 Logger    = source.["logger"],
                                 CreatedAt = match DateTime.TryParse(source.["createdAt"]) with
                                                | (true, creationDate) -> new Nullable<DateTime>(creationDate)
                                                | (false, _) -> Unchecked.defaultof<_>)

        base.ExecuteBindingAsync(metadataProvider, actionContext, cancellationToken)

    member private me.TryParseAsJson (value: string): LogModel =
        try 
            let json = JsonConvert.DeserializeObject<LogModel>(value)
            if (json = Unchecked.defaultof<_>) then broken
            else json
        with
            | :? JsonReaderException| :? JsonSerializationException -> broken
            | _ as ex -> raise(ex)


[<AttributeUsage(AttributeTargets.Method)>]
type LogModelBinderAttribute() =
    inherit ParameterBindingAttribute()

    member val Target: string = "logModel" with get, set
    member val Source: string = "formData" with get, set

    override me.GetBinding(parameter) =
        new LogModelParameterBinding(me.Target, me.Source, parameter) :> HttpParameterBinding
