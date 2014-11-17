namespace NBlast.Storage.Core.Index

type IPaginator = interface
    abstract member GetFollowingSection: (int) -> (int) -> (int) -> seq<int>
    end