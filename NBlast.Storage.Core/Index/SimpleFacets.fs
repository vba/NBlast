namespace NBlast.Storage.Core.Index

open Newtonsoft.Json

type SimpleFacet = 
    { [<field: JsonProperty("name")>]   Name    : string
      [<field: JsonProperty("count")>]  Count   : int64 }

type SimpleFacets = 
    { [<field: JsonProperty("facets")>]         Facets          : SimpleFacet seq
      [<field: JsonProperty("queryDuration")>]  QueryDuration   : int64 
      [<field: JsonProperty("total")>]          Total           : int }

    static member GetEmpty() = {Facets = []; QueryDuration = 0L; Total = 0}