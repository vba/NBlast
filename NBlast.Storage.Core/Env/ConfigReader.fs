namespace NBlast.Storage.Core

open System
open NLog
open System.Configuration


type AppSettingNotFoundException = 
    inherit InvalidOperationException
    new (key) = { inherit InvalidOperationException(sprintf "App setting not found for key %s" key) } 

type AppSettingTryCastException = 
    inherit InvalidOperationException
    new (key, ``type``) = { inherit InvalidOperationException(sprintf "Cannot conver setting %s to %A" key ``type``) } 



type IConfigReader = interface
    abstract member Read: string -> string
    abstract member ReadAsInt: string -> int
end


type ConfigReader() = 
    static let logger = NLog.LogManager.GetCurrentClassLogger()

    interface IConfigReader with 
        member me.Read key =
            try
                ConfigurationManager.AppSettings.[key] |> Environment.ExpandEnvironmentVariables
            with
            | _ -> raise (new AppSettingNotFoundException(key))

        member me.ReadAsInt key =
            match key |> (me :> IConfigReader).Read |> Int32.TryParse with
            | (false, _) -> raise (new AppSettingTryCastException(key, int32.GetType()))
            | (true, value) -> value
