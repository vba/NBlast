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
open System.Net.Http.Formatting

type JsonpMediaFormatterSpecs() =


    member private me.MakeSut(?formatter:MediaTypeFormatter) = 
        new JsonpMediaTypeFormatter(defaultArg formatter (new JsonMediaTypeFormatter() :> MediaTypeFormatter))

    member private me.MakeSut(formatter:JsonpMediaTypeFormatter, callback, callbackQueryParameter) = 
        new JsonpMediaTypeFormatter(formatter, callback, callbackQueryParameter)