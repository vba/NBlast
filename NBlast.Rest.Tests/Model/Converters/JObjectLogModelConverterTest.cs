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
            //var jobject = CreateSimpleEbeddedJsonObject();

            // 
        }

        private JObject CreateSimpleEbeddedJsonObject()
        {
            var jsonString = @"
            { events:[{
                Timestamp:'2015-10-08T13:37:12.9486666+02:00'
                  Level:Debug,
                  MessageTemplate: 'Some {id} {@obj},'
                  Properties:{
                    id:1,
                    obj: {
                        _typeTag:Entity,
                        Id:level2,
                        Parent:{
                            '_typeTag': 'Entity',
                            Id: 'first',
                            Parent:null,
                            Children:[]
                        },
                        Children:[{
                          '_typeTag':'Entity',
                          'Id':'level3',
                          Parent:null,
                          Children:[]
                        }]
                    }
                  }
              }]}";
            return JsonConvert.DeserializeObject<JObject>(jsonString);
        }
    }
}
