namespace NBlast.Storage.Core

type LogDocumentHit = 
    { Sender   : string
      Error    : string
      Message  : string
      Logger   : string
      Level    : string
      Boost    : float32
      CreatedAt: System.DateTime
      Score    : float32 }

