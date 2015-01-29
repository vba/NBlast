namespace NBlast.Storage.Core.Index

type LogField = 
    | Id
    | Sender
    | Content
    | Error
    | Message
    | Logger
    | Level
    | CreatedAt
    with 
        member me.GetName() = 
            match me with
            | Id        -> "id"
            | Sender    -> "sender"
            | Error     -> "error"
            | Message   -> "message"
            | Logger    -> "logger"
            | Level     -> "level"
            | Content   -> "content"
            | CreatedAt -> "createdAt"

        member me.QueryWith query = me.GetName() + ": "+ query
        
        static member Names = 
            [| LogField.Sender.GetName();
               LogField.Id.GetName();
               LogField.Content.GetName();
               LogField.Message.GetName();
               LogField.Logger.GetName();
               LogField.Level.GetName();
               LogField.CreatedAt.GetName();
               LogField.Error.GetName(); |]

        static member ConvertFrom(key: string) = 
            let key = if key = null then "" else key.ToUpperInvariant()
            match key with
            |"ID"        -> LogField.Id |> Some
            |"SENDER"    -> LogField.Sender |> Some
            |"ERROR"     -> LogField.Error |> Some
            |"MESSAGE"   -> LogField.Message |> Some
            |"LOGGER"    -> LogField.Logger |> Some
            |"LEVEL"     -> LogField.Level |> Some
            |"CONTENT"   -> LogField.Content |> Some
            |"CREATEDAT" -> LogField.CreatedAt |> Some
            | _ -> None
