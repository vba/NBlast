using System;
using System.Collections.Generic;
using System.Collections.Immutable;

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

        public ISet<Tuple<string, object>> Properties { get; set; } = new HashSet<Tuple<string, object>>().ToImmutableHashSet();
        public ISet<Tuple<string, string>> TemplateTokensTexts { get; set; } = new HashSet<Tuple<string, string>>().ToImmutableHashSet();
        public ISet<Tuple<string, string>> TemplateTokensProperties { get; set; } = new HashSet<Tuple<string, string>>().ToImmutableHashSet();

    }
}