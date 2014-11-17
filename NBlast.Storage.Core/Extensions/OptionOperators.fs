namespace NBlast.Storage.Core.Extensions

    [<AutoOpen>]
    module OptionOperators =
        let inline (|?) (a: 'a option) b = if a.IsSome then a.Value else b