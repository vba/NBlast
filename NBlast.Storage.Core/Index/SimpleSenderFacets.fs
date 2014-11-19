namespace NBlast.Storage.Core.Index

type SimpleSenderFacet = { Name : string; Count: int64}
type SimpleSenderFacets = { Facets : SimpleSenderFacet list; QueryDuration: int64}