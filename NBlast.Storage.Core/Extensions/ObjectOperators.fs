namespace NBlast.Storage.Core.Extensions

    [<AutoOpen>]
    module ObjectOperators =
        let inline ``is ∅`` (x:^a when ^a : not struct) =
            obj.ReferenceEquals (x, Unchecked.defaultof<_>) 

