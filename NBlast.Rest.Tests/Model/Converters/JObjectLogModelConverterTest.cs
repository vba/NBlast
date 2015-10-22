using FluentAssertions;
using NBlast.Rest.Model.Converters;
using NBlast.Rest.Model.Dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace NBlast.Rest.Tests.Model.Converters
{
    public class JObjectLogModelConverterTest
    {
        [Fact(DisplayName ="It should covert a simple embedded object to its valid log event representation")]
        public void Check_Convert_on_simple_embedded_object()
        {
            // given
            var jObject   = CreateSimpleEbeddedJsonObject();
            var sut       = new JObjectLogModelConverter();
            var propsKey  = nameof(LogEvent.Properties);
            var guidRegex = new Regex(@"\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-F0-9]{12}\b", RegexOptions.IgnoreCase);

            // when
            var logEvent = sut.Convert(jObject);

            // then
            logEvent.Level.Should().Be("Debug");
            logEvent.Timestamp.Should().BeCloseTo(new DateTime(2015, 10, 8, 13, 37, 12, 948));
            logEvent.MessageTemplate.Should().Be("Some {id} {@obj},");

            GetPropValue<string>(logEvent, "name").Should().Be("Bob");
            GetPropValue<object>(logEvent, "prop2").Should().BeNull();
            GetPropValue<DateTime?>(logEvent, "prop1").Should().BeCloseTo(new DateTime(1970, 1, 1));
            guidRegex.IsMatch(GetPropValue<string>(logEvent, "id")).Should().BeTrue();
        }

        [Fact(DisplayName ="It should covert an embedded object with cyclic references to its valid log event representation")]
        public void Check_Convert_with_cyclic_references()
        {
            // given
            var jObject   = CreateEbeddedJsonObjectWithCyclicReferences();
            var sut       = new JObjectLogModelConverter();
            var guidRegex = new Regex(@"\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-F0-9]{12}\b", RegexOptions.IgnoreCase);

            // when
            var logEvent = sut.Convert(jObject);

            // then
            logEvent.Properties.Any().Should().BeTrue();
            GetPropValue<string>(logEvent, "cyclics.Name").Should().Be("Bill");
            GetPropValue<string>(logEvent, "cyclics.Brother.Name").Should().Be("Bob");
            GetPropValue<string>(logEvent, "cyclics.$ref").Should()
                .Be(GetPropValue<string>(logEvent, "cyclics.Brother.$id"));
            GetPropValue<string>(logEvent, "cyclics.Brother.Brother.$ref").Should()
                .Be(GetPropValue<string>(logEvent, "cyclics.$id"));
        }

        [Fact(DisplayName ="It should covert a complex embedded object to its valid log event representation")]
        public void Check_Convert_with_complex_embedded_object()
        {
            // given
            var obj = new
            {
                _typeTag = "Entity",
                Id = "Level2",
                Parent = new
                {
                    _typeTag = "Entity",
                    Id = "first",
                    Parent = new { },
                    Children = new object[0]
                },
                Children = new[]
                {
                    new
                    {
                        _typeTag = "Entity1",
                        Id = "level1",
                        Parent = new object(),
                        Children = new object[0]
                    },
                    new
                    {
                        _typeTag = "Entity2",
                        Id = "level2",
                        Parent = new object(),
                        Children = new object[0]
                    },
                    new
                    {
                        _typeTag = "Entity3",
                        Id = "level3",
                        Parent = new object(),
                        Children = new object[0]
                    },
                }
            };
            var sut       = new JObjectLogModelConverter();
            var propsKey  = nameof(LogEvent.Properties);
            var guidRegex = new Regex(@"\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-F0-9]{12}\b", RegexOptions.IgnoreCase);
            var jObject   = CreateEmbeddedJsonObject(obj);

            // when
            var logEvent = sut.Convert(jObject);

            // then
            logEvent.Properties.Any().Should().BeTrue();
            GetPropValue<long>(logEvent, "id").Should().Be(1);
            GetPropValue<string>(logEvent, "obj._typeTag").Should().Be(obj._typeTag);
            GetPropValue<string>(logEvent, "obj.Id").Should().Be(obj.Id);
            GetPropValue<string>(logEvent, "obj.Parent._typeTag").Should().Be("Entity");
            GetPropValue<string>(logEvent, "obj.Parent.Id").Should().Be("first");
            GetPropValue<string>(logEvent, "obj.Parent._typeTag").Should().Be("Entity");
            logEvent.Properties.Where(x => x.Key.EndsWith("Children.Id"))
                .Select(x => x.Value.ToString())
                .SequenceEqual(obj.Children.Select(x => x.Id)).Should()
                .BeTrue();
            logEvent.Properties.Where(x => x.Key.EndsWith("Children._typeTag"))
                .Select(x => x.Value.ToString())
                .SequenceEqual(obj.Children.Select(x => x._typeTag)).Should()
                .BeTrue();
        }

        private static T GetPropValue<T>(LogEvent logEvent, string key) =>
            (T)logEvent
                .Properties
                .FirstOrDefault(x => x.Key == $"{nameof(LogEvent.Properties)}.{key}")?.Value;


        private JObject CreateSimpleEbeddedJsonObject()
        {
            var jsonString = JsonConvert.SerializeObject(new
            {
                events = new[]
                {
                    new
                    {
                        Timestamp = "2015-10-08T13:37:12.9486666+02:00",
                        Level = "Debug",
                        MessageTemplate = "Some {id} {@obj},",
                        Properties = new {
                            prop1 = new DateTime(1970, 1, 1),
                            prop2 = new int?(),
                            name = "Bob",
                            id = Guid.NewGuid()

                        }
                    }
                }
            });

            return JsonConvert.DeserializeObject<JObject>(jsonString);
        }
        private JObject CreateEbeddedJsonObjectWithCyclicReferences()
        {
            dynamic cyclicObject1 = new ExpandoObject();
            var cyclicObject2 = new { Brother = cyclicObject1, Name = "Bob" };

            cyclicObject1.Name = "Bill";
            cyclicObject1.Brother = cyclicObject2;


            var jsonString = JsonConvert.SerializeObject(new
            {
                events = new[]
                {
                    new
                    {
                        Timestamp = "2015-10-08T13:37:12.9486666+02:00",
                        Level = "Debug",
                        MessageTemplate = "Some {id} {@obj},",
                        Properties = new {
                            cyclics = new [] {cyclicObject1, cyclicObject2},
                        }
                    }
                }
            },
            new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            });

            return JsonConvert.DeserializeObject<JObject>(jsonString); ;
        }

        private JObject CreateEmbeddedJsonObject(object obj)
        {
            var jsonString = JsonConvert.SerializeObject(new {
                events = new[]
                {
                    new
                    {
                        Timestamp = "2015-10-08T13:37:12.9486666+02:00",
                        Level = "Debug",
                        MessageTemplate = "Some {id} {@obj},",
                        Properties = new
                        {
                            id = 1,
                            obj = obj
                        }
                    }
                }
            }, 
            new JsonSerializerSettings {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
                
            return JsonConvert.DeserializeObject<JObject>(jsonString);
        }
    }
}
