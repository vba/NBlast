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

namespace NBlast.Rest.Model.Converters
{

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

        public IImmutableDictionary<string, object> ConvertToMap(JObject jObject)
        {
            var list   = new List<Pair>();
            var cursor = new List<Tuple<string, IEnumerable<JProperty>>> { Tuple.Create("", jObject["events"].SelectMany(x => x.Children<JProperty>())) };

            do
            {
                var newCursor = new List<Tuple<string, IEnumerable<JProperty>>>();
                foreach (var tuple in cursor)
                {
                    list.AddRange(tuple.Item2
                        .Where(x => !x.Value.HasValues)
                        .Select(x => new Pair($"{tuple.Item1}{x.Name}", x.Value))
                    );
                    newCursor.AddRange(
                        tuple.Item2
                            .Where(x => x.Value.HasValues)
                            .Select(x => Tuple.Create($"{tuple.Item1}{x.Name}.", x.SelectMany(y => y.Children<JProperty>())
                   )));
                }

                if (!newCursor.Any()) {
                    return list.ToImmutableDictionary();
                }

                cursor = newCursor;
                continue;

            } while (true);
        }
    }
}
