namespace NBlast.Storage.Core.Index

type LogDocumentHit = 
    { Sender   : string
      Error    : string
      Message  : string
      Logger   : string
      Level    : string
      Boost    : float32
      CreatedAt: System.DateTime
      Score    : float32 option }

