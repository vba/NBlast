namespace NBlast.Api.Tests.Formatting.Jsonp

open NBlast.Api.Formatting.Jsonp
open System
open System.Web.Http
open System.Runtime
open NUnit.Framework
open FluentAssertions
open Ploeh.AutoFixture
open Ploeh.AutoFixture.Kernel
open System.Threading.Tasks
open System.Net.Http
open System.Net.Http.Handlers
open System.Net.Http.Formatting
open Newtonsoft.Json.Linq

type ValueController() = 
    inherit ApiController() 
    member me.Get(id: int) = 
        "value 1"

type DefaultRoute = {id: RouteParameter}

type JsonpMediaTypeFormatterSpecs() =

    [<Test>]
    member me.``Formatter can read type method should always return false``() =
        let formatter = me.MakeSut()
        formatter.CanReadType(typeof<Object>).Should().BeFalse("Always false") |> ignore
        formatter.CanReadType(typeof<String>).Should().BeFalse("Always false") |> ignore
        formatter.CanReadType(typeof<JToken>).Should().BeFalse("Always false") |> ignore

    [<TestCase("POST")>]
    [<TestCase("PUT")>]
    [<TestCase("DELETE")>]
    [<TestCase("HEAD")>]
    [<TestCase("OPTIONS")>]
    [<TestCase("TRACE")>]
    member me.``Formatter should not find a callback where it's impossible to find``(httpMethod) = 
        let httpMethod = new HttpMethod(httpMethod)
        let request = new HttpRequestMessage(httpMethod, "http://example.org/")
        let callback = JsonpMediaTypeFormatter.GetJsonpCallback request "callback"
        callback.IsNone.Should().BeTrue("Must be none") |> ignore

    [<Test>]
    member me.``Formatter should find a callback when it's passed in url``() = 
        let request = new HttpRequestMessage(HttpMethod.Get, "http://example.org/?callback=param")
        let callback = JsonpMediaTypeFormatter.GetJsonpCallback request "callback"
        callback.IsSome.Should().BeTrue("Must be some") |> ignore
        callback.Value.Should().Be("param", "Must be param") |> ignore

//    [<Ignore>]
    [<TestCase("/api/value/1", "", "application/json", "\"value 1\"")>]
    [<TestCase("/api/value/1", "application/json", "application/json", "\"value 1\"")>]
    [<TestCase("/api/value/1?callback=?", "", "text/javascript", "?(\"value 1\");")>]
    [<TestCase("/api/value/1?callback=?", "application/json", "text/javascript", "?(\"value 1\");")>]
    // text/javascript
    [<TestCase("/api/value/1?callback=?", "text/javascript", "text/javascript", "?(\"value 1\");")>]
    [<TestCase("/api/value/1", "text/javascript", "text/javascript", "A callback parameter was not provided in the request URI.")>]
    // application/javascript
    [<TestCase("/api/value/1?callback=?", "application/javascript", "text/javascript", "?(\"value 1\");")>]
    [<TestCase("/api/value/1", "application/javascript", "text/javascript", "A callback parameter was not provided in the request URI.")>]
    // application/json-p
    [<TestCase("/api/value/1?callback=?", "application/json-p", "text/javascript", "?(\"value 1\");")>]
    [<TestCase("/api/value/1", "application/json-p", "text/javascript", "A callback parameter was not provided in the request URI.")>]
    member me.``Formatter should write to stream expected results``(requestUri: string, 
                                                                    acceptMediaType: string,
                                                                    expectedMediaType: string,
                                                                    expectedValue: string) = 
        let config = new HttpConfiguration()
        config.Formatters.Add(me.MakeSut(config.Formatters.JsonFormatter))
        config.Routes.MapHttpRoute("Default", "api/{controller}/{id}", { id = RouteParameter.Optional }) |> ignore;

        use server = new HttpServer(config)
        use client = new HttpClient(server)
        
        client.BaseAddress <- new Uri("http://test.org/")
        let request = new HttpRequestMessage(HttpMethod.Get, requestUri)

        if (not(String.IsNullOrEmpty(acceptMediaType))) then
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(acceptMediaType))

        try
            let response = client.SendAsync(request).Result
            let content  = response.Content
            expectedMediaType.Should().Be(content.Headers.ContentType.MediaType, "should be the same") |> ignore
            
            let text = content.ReadAsAsync().Result
            expectedValue.Should().Be(text, "Text should be the same") |> ignore

        with 
            | :? AggregateException as ex -> expectedValue.Should().Be(ex.InnerException.Message, "Must be the same") |> ignore


    member private me.MakeSut(?formatter:MediaTypeFormatter) : JsonpMediaTypeFormatter = 
        new JsonpMediaTypeFormatter(defaultArg formatter (new JsonMediaTypeFormatter() :> MediaTypeFormatter))

    member private me.MakeSut(formatter:JsonpMediaTypeFormatter, callback, callbackQueryParameter) = 
        new JsonpMediaTypeFormatter(formatter, callback, callbackQueryParameter)