using System;
using System.Collections.Generic;

namespace NBlast.Rest.Model.Write
{
    public class LogEntry
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Level { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public string Data { get; set; }
        public string Content { get; set; }

        public string Exception { get; set; }
    }
}