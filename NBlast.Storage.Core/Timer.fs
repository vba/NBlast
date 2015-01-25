namespace NBlast.Storage.Core

open System.Diagnostics

type Timer<'a>(closure: unit -> 'a) =
    let sw = new Stopwatch()
    member me.WrapExecution() =
        sw.Start()
        let result = closure()
        sw.Stop()
        result
    
    member me.GetElapsedMilliseconds() =
        sw.ElapsedMilliseconds