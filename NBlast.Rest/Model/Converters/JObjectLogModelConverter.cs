using Equ;
using NBlast.Rest.Model.Dto;
using NBlast.Rest.Services.Write;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pair = System.Collections.Generic.KeyValuePair<string, object>;
using static System.String;

namespace NBlast.Rest.Model.Converters
{

    public class LogEventItem : IEquatable<LogEventItem>
    {
        private static readonly MemberwiseEqualityComparer<LogEventItem> Comparer = MemberwiseEqualityComparer<LogEventItem>.ByProperties;

        public Guid Id { get; } = Guid.NewGuid();
        public string Key { get; }
        public object Value { get;}

        public LogEventItem(string key, object value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));

            Key = key;
            Value = value;
        }

        public bool Equals(LogEventItem other)
        {
            return Comparer.Equals(other);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as LogEventItem);
        }

        public override int GetHashCode()
        {
            return Comparer.GetHashCode(this);
        }

        public override string ToString()
        {
            return $"(key = {Key}, value = {Value}, id = {Id})";
        }
    }

    public interface IJObjectLogModelConverter : IConverter<JObject, LogEvent>
    {
    }

    public class JObjectLogModelConverter : IJObjectLogModelConverter
    {
        public LogEvent Convert(JObject logModel)
        {
            // TODO: decompose JArrays -> Field1 = 1, Field1 = 2
            // TODO: decompose JObjects
            // TODO: Stop using dictionary and migrate to custom type with duplicated keys -> FieldsMap 

            throw new NotImplementedException();
        }

        public IImmutableList<LogEventItem> Deserialize(string json)
        {
            return ToObject(JToken.Parse(json));
        }

        public IImmutableList<LogEventItem> ToObject(JToken token, string prefix = "")
        {
            var keyPart = IsNullOrEmpty(prefix) ? "" : $"{prefix}.";
            switch (token.Type)
            {
                case JTokenType.Object:
                    return token.Children<JProperty>()
                                .SelectMany(x => ToObject(x.Value, $"{keyPart}{x.Name}" ))
                                .ToImmutableList();

                case JTokenType.Array:
                    return token.SelectMany( x => ToObject(x, prefix)).ToImmutableList();

                default:
                    return new[] {new LogEventItem(prefix, ((JValue)token).Value) }.ToImmutableList() ;
            }
        }



        public IImmutableList<LogEventItem> ConvertToMap(JObject jObject)
        {
            var list   = new List<LogEventItem>();
            var cursor = new List<Tuple<string, IList<JProperty>>> { Tuple.Create("", jObject["events"].SelectMany(x => x.Children<JProperty>()).ToList() as IList<JProperty>) };

            do
            {
                var newCursor = new List<Tuple<string, IList<JProperty>>>();
                foreach (var tuple in cursor)
                {
                    list.AddRange(tuple.Item2
                        .Where(x => !x.Value.HasValues)
                        .Select(x => new LogEventItem ($"{tuple.Item1}{x.Name}", x.Value.ToObject<object>()))
                    );
                    var lst = tuple.Item2.Where(x => x.Value.HasValues).ToList();

                    newCursor.AddRange(
                        tuple.Item2
                            .Where(x => x.Value.HasValues)
                            .Select(x => Tuple.Create($"{tuple.Item1}{x.Name}.", 
                                                      x.SelectMany(y => {
                                                        return y.Children<JProperty>().ToList();
                                                      }).ToList() as IList<JProperty>)
                   ));
                }

                if (!newCursor.Any()) {
                    return list.ToImmutableList();
                }

                cursor = newCursor;
                continue;

            } while (true);
        }
    }
}
