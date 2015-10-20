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
        [Fact]
        public void Check_Convert_on_simple_embedded_object()
        {
            // given
            var jObject = CreateSimpleEbeddedJsonObject();
            var sut = new JObjectLogModelConverter();
            var propsKey = nameof(LogEvent.Properties);
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

        private JObject _CreateSimpleEbeddedJsonObject()
        {
            dynamic cyclicObject1 = new ExpandoObject();
            var cyclicObject2 = new { Brother = cyclicObject1, Name = "Bob" };

            cyclicObject1.Name = "Bill";
            cyclicObject1.Brother = cyclicObject2;
             
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
                            cyclics = new [] {cyclicObject1, cyclicObject2}, 
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
                                	    _typeTag = "Entity",
                                	    Id = "level3",
                                	    Parent = new object(),
                                	    Children = new object[0]
                                	}
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
