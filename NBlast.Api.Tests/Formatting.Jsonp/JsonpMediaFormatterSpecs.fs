namespace NBlast.Api.Tests.Formatting.Jsonp

open NBlast.Api.Formatting.Jsonp
open System
open System.Runtime
open Xunit
open FluentAssertions
open Ploeh.AutoFixture
open Ploeh.AutoFixture.Kernel
open System.Threading.Tasks
open System.Net.Http

type JsonpMediaFormatterSpecs() =

    member private me.MakeSut(formatter:JsonpMediaTypeFormatter) = 0
        //new JsonpMediaTypeFormatter(formatter) :> JsonpMediaTypeFormatter