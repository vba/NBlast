namespace NBlast.Storage.Core.Extensions

    [<AutoOpen>]
    module OptionOperators =
        let inline (|?) (a: 'a option) b = 
            match a with
            | Some a -> a
            | _ -> b