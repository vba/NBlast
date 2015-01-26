namespace NBlast.Storage.Core.Index

open System

type Sort = 
    { Field: LogField
      Reverse: bool }

    static member OnlyField field = 
        { Field = field
          Reverse = false }

