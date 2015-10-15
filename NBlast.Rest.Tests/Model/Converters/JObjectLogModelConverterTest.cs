using NBlast.Rest.Model.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
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

            // when
            //var dictionary = sut.ConvertToMap(jObject);
            var lists = jObject["events"].Select(x => sut.ToObject(x)).ToList();

        }

        private JObject CreateSimpleEbeddedJsonObject()
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
