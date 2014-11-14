namespace NBlast.Storage.Tests.FileSystem


open NBlast.Storage.Core
open NBlast.Storage.FileSystem
open Xunit
open FluentAssertions

type StorageReaderTest() = 

    member private this.MakeSut path :IStorageReader =
        new StorageReader(path) :> IStorageReader

