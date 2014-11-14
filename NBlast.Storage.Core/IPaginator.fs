namespace NBlast.Storage.Core

type IPaginator = interface
    abstract member GetFollowingSection: (int) -> (int) -> (int) -> seq<int>
    end