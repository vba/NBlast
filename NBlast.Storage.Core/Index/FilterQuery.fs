namespace NBlast.Storage.Core.Index

open System

type FilterQuery = 
    | After of DateTime
    | Before of DateTime
    | Between of DateTime * DateTime