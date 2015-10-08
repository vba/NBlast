using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace NBlast.Rest.Console
{

    class Entity
    {
        public String Id { get; set; }
        public Entity Parent { get; set; }
        public Entity[] Children { get; set; } = new Entity[0];
    }
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(@"c:\temp\nblast.serilog.log")
                .WriteTo.Seq("http://localhost.fiddler:9090/")
//                .WriteTo.Elasticsearch("http://localhost.fiddler:9090/")
                .MinimumLevel.Verbose()
                .CreateLogger();

            var parent = new Entity {Id = "first"};
            var obj = new Entity
            {
                Id = "level2",
                Parent = parent,
                Children = new []{new Entity {Id = "level3"}, }
            };

            Log.Logger.Debug("Some {id} {@obj}", 1, obj);
            //            Log.Logger.Warning("Some {id}", 1);

            /*
            SEQ:                        
            {
              "events":[
                {
                  "Timestamp":"2015-10-08T13:37:12.9486666+02:00",
                  "Level":"Debug",
                  "MessageTemplate":"Some {id} {@obj}",
                  "Properties":{
                    "id":1,
                    "obj":{
                      "_typeTag":"Entity",
                      "Id":"level2",
                      "Parent":{
                        "_typeTag":"Entity",
                        "Id":"first",
                        "Parent":null,
                        "Children":[
                        ]
                      },
                      "Children":[
                        {
                          "_typeTag":"Entity",
                          "Id":"level3",
                          "Parent":null,
                          "Children":[
                          ]
                        }
                      ]
                    }
                  }
                }
              ]
            }
            
            Elastic:

            {
              "@timestamp":"2015-10-08T13:32:26.3767970+02:00",
              "level":"Debug",
              "messageTemplate":"Some {id} {@obj}",
              "message":"Some 1 Entity { Id: \"level2\", Parent: Entity { Id: \"first\", Parent: null, Children: [] }, Children: [Entity { Id: \"level3\", Parent: null, Children: [] }] }",
              "fields":{
                "id":1,
                "obj":{
                  "_typeTag":"Entity",
                  "Id":"level2",
                  "Parent":{
                    "_typeTag":"Entity",
                    "Id":"first",
                    "Parent":null,
                    "Children":[
                    ]
                  },
                  "Children":[
                    {
                      "_typeTag":"Entity",
                      "Id":"level3",
                      "Parent":null,
                      "Children":[
                      ]
                    }
                  ]
                }
              }
            }
            */
        }
    }
}
