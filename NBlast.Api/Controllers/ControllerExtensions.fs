namespace NBlast.Api.Controllers

open System
open System.Web.Http
open System.Net.Http.Formatting
open NBlast.Storage.Core.Index
open NBlast.Storage.Core
open NBlast.Storage.Core.Extensions

[<AutoOpen>]
module ApiControllerExtensions =
    type ApiController with
        member private me.ComposeExpression q = 
            SearchQuery.GetOnlyExpression(q)

        member private me.ComposePage p (take: Lazy<int>) query = 
            { query with Take = take.Value |> Some 
                         Skip = ((p - 1) * take.Value) |> Some }

        member private me.ComposeSort sf (sr: Nullable<Boolean>) query = 
            let sf = LogField.ConvertFrom(sf)
            { query with Sort = if sf.IsNone then None
                                else { Field   = sf.Value 
                                       Reverse = if sr.HasValue 
                                                 then sr.Value 
                                                 else false} |> Some }

        member private me.ComposeFilter (from: Nullable<DateTime>) 
                                        (till: Nullable<DateTime>) 
                                        query = 
            { query with 
                Filter = 
                    if (from.HasValue && till.HasValue) 
                        then FilterQuery.Between(from.Value, till.Value) |> Some
                    else if (from.HasValue)
                        then FilterQuery.After(from.Value)  |> Some
                    else if (till.HasValue)
                        then FilterQuery.Before(till.Value) |> Some
                    else None
            }

        member me.AssembleSearchQuery (q, p, take, sf, sr, from, till) = 
            q |> (me.ComposeExpression 
                    >> (me.ComposePage p take) 
                    >> (me.ComposeSort sf sr) 
                    >> (me.ComposeFilter from till))