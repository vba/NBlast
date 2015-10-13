using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
            var jobject = CreateSimpleEbeddedJsonObject();

            // 
        }

        private JObject CreateSimpleEbeddedJsonObject()
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
            });
                
            return JsonConvert.DeserializeObject<JObject>(jsonString);
        }
    }
}
