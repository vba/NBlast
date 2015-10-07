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
        }
    }
}
