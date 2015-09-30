using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace NBlast.Rest.Model.Write
{
    public class LogEntry
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Level { get; }
        public DateTime CreationDate { get; }
        public string Data { get; }
        public string Content { get { return string.Empty; } }
        public string Exception { get; }

        public ISet<LogEntryProperty> Properties { get;}
        public ISet<string> TemplateTokensTexts { get;}
        public ISet<string> TemplateTokensProperties { get; }

        public LogEntry(string level,
                        string data,
                        DateTime? creationDate                = null,
                        string exception                      = null,
                        ISet<LogEntryProperty> properties     = null,
                        ISet<string> templateTokensTexts      = null,
                        ISet<string> templateTokensProperties = null)
        {
            Level                    = level;
            Data                     = data;
            Exception                = exception;
            CreationDate             = creationDate ?? DateTime.UtcNow;
            Properties               = properties ?? Enumerable.Empty<LogEntryProperty>().ToImmutableHashSet();
            TemplateTokensTexts      = templateTokensTexts ?? Enumerable.Empty<string>().ToImmutableHashSet();
            TemplateTokensProperties = templateTokensProperties ?? Enumerable.Empty<string>().ToImmutableHashSet();
        }

        protected bool Equals(LogEntry other)
        {
            return Id.Equals(other.Id) 
                && string.Equals(Level, other.Level) 
                && CreationDate.Equals(other.CreationDate) 
                && string.Equals(Data, other.Data) 
                && string.Equals(Exception, other.Exception) 
                && Equals(Properties, other.Properties) 
                && Equals(TemplateTokensTexts, other.TemplateTokensTexts) 
                && Equals(TemplateTokensProperties, other.TemplateTokensProperties);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((LogEntry) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (Level?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ CreationDate.GetHashCode();
                hashCode = (hashCode * 397) ^ (Data?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Exception?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Properties?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (TemplateTokensTexts?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (TemplateTokensProperties?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        public static bool operator ==(LogEntry left, LogEntry right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LogEntry left, LogEntry right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"Id: {Id}, Level: {Level}, CreationDate: {CreationDate}, Data: {Data}, Exception: {Exception}, Properties: {Properties}, TemplateTokensTexts: {TemplateTokensTexts}, TemplateTokensProperties: {TemplateTokensProperties}";
        }
    }
}