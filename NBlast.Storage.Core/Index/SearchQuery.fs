namespace NBlast.Storage.Core.Index

open NBlast.Storage.Core.Extensions

type SearchQuery =
    { Expression: string
      Filter    : string option
      Skip      : int option
      Take      : int option }

    static member GetOnlyExpression ?expression = 
        let expression = expression |? "*:*"
        
        { Expression = expression
          Filter     = None
          Skip       = None
          Take       = None }

