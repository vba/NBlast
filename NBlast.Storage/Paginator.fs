namespace NBlast.Storage

open NBlast.Storage.Core


type Paginator() =
    interface IPaginator with
        member this.GetFollowingSection skip take total =
            if (skip >= total) 
                then Seq.empty
            else if ((skip + take) <= total)
                then seq {skip + 1 .. skip + take}
            else
                seq {skip + 1 .. total}
