using Equ;
using NBlast.Rest.Model.Dto;
using NBlast.Rest.Services.Write;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Immutable;
using System.Linq;
using static System.String;

namespace NBlast.Rest.Model.Converters
{

    public interface IJObjectLogModelConverter : IConverter<JObject, LogEvent>
    {
    }

    public class JObjectLogModelConverter : IJObjectLogModelConverter
    {
        public LogEvent Convert(JObject jObject)
        {
            // TODO: decompose JArrays -> Field1 = 1, Field1 = 2
            // TODO: decompose JObjects
            // TODO: Stop using dictionary and migrate to custom type with duplicated keys -> FieldsMap 

            var deserialized = Deserialize(jObject);
            //var result = new LogEvent(deserialized.FirstOrDefault(x => x.Key == nameof(LogEvent.Level)).Value,
            //                          deserialized.FirstOrDefault(x => x.Key == nameof(LogEvent.Exception))?.Value,
            //                          deserialized.FirstOrDefault(x => x.Key == nameof(LogEvent.MessageTemplate))?.Value,
            //                          )
            return null;
        }

        private IImmutableList<LogEventItem> Deserialize(string json)
        {
            return Deserialize(JToken.Parse(json));
        }

        private IImmutableList<LogEventItem> Deserialize(JToken token)
        {
            const string eventsKey = "events";
            return token.Children<JProperty>().Any(x => x.Name == eventsKey)
                ? ToObject(token[eventsKey])
                : ToObject(token);
        }

        private IImmutableList<LogEventItem> ToObject(JToken token, string prefix = "")
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
    }
}
