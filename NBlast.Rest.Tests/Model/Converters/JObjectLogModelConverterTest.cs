using FluentAssertions;
using NBlast.Rest.Model.Converters;
using NBlast.Rest.Model.Dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
            logEvent.Properties.First(x => x.Key == $"{propsKey}.name").Value.Should().Be("Bob");
            logEvent.Properties.First(x => x.Key == $"{propsKey}.prop2").Value.Should().BeNull();
            guidRegex.IsMatch(logEvent.Properties.First(x => x.Key == $"{propsKey}.id").Value.ToString())
                .Should()
                .BeTrue();
            (logEvent.Properties.First(x => x.Key == $"{propsKey}.prop1").Value as DateTime?)
                .Should()
                .BeCloseTo(new DateTime(1970, 1, 1));

        }

         [Fact(DisplayName ="It should covert an embedded object with cyclic references to its valid log event representation")]
        public void Check_Convert_with_cyclic_references()
        {
            // given
            var jObject   = CreateEbeddedJsonObjectWithCyclicReferences();
            var sut       = new JObjectLogModelConverter();
            var propsKey  = nameof(LogEvent.Properties);
            var guidRegex = new Regex(@"\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-F0-9]{12}\b", RegexOptions.IgnoreCase);

            // when
            var logEvent = sut.Convert(jObject);

            // then
            logEvent.Properties.Any().Should().BeTrue();
            logEvent.Properties.First(x => x.Key == $"{propsKey}.cyclics.Name").Value.Should().Be("Bill");
            logEvent.Properties.First(x => x.Key == $"{propsKey}.cyclics.Brother.Name").Value.Should().Be("Bob");
            logEvent.Properties.First(x => x.Key == $"{propsKey}.cyclics.$ref").Value.Should()
                .Be(logEvent.Properties.First(x => x.Key == $"{propsKey}.cyclics.Brother.$id").Value);
            logEvent.Properties.First(x => x.Key == $"{propsKey}.cyclics.Brother.Brother.$ref").Value.Should()
                .Be(logEvent.Properties.First(x => x.Key == $"{propsKey}.cyclics.$id").Value);
        }

        [Fact(DisplayName ="It should covert a complex embedded object to its valid log event representation")]
        public void Check_Convert_with_complex_embedded_object()
        {
            // given
            var jObject   = CreateComplexEmbeddedJsonObject();
            var sut       = new JObjectLogModelConverter();
            var propsKey  = nameof(LogEvent.Properties);
            var guidRegex = new Regex(@"\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-F0-9]{12}\b", RegexOptions.IgnoreCase);

            // when
            var logEvent = sut.Convert(jObject);

            // then
            logEvent.Properties.Any().Should().BeTrue();
       }
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

        private JObject CreateComplexEmbeddedJsonObject()
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
                            obj = new
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
                                Children = new [] 
                                {
                                    new
                                    {
                                	    _typeTag = "Entity1",
                                	    Id = "level3",
                                	    Parent = new object(),
                                	    Children = new object[0]
                                	},
                                    new
                                    {
                                	    _typeTag = "Entity2",
                                	    Id = "level3",
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
                            }
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
