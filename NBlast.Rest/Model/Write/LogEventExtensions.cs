using LanguageExt;
using System.Collections.Immutable;
using System.Linq;
using static LanguageExt.Prelude;
using static System.String;

namespace NBlast.Rest.Model.Write
{
    public static class LogEventExtensions
    {
        public const string ErrorEmptyLevelField = "Level field value can not be empty";
        public const string ErrorNoPropsAndMessage = "Message and properties field can not be empty at the same time";

        private static Option<string[]> AppendAggregation(Option<string[]> subject, string error) =>
            subject.Match(Some: x => x.Union(new[] { error }).ToArray(),
                          None: () => new[] { error });
        public static Option<string[]> Validate(this LogEvent me)
        {
            return new []
            {
                (me.Level?.Trim()?.Length ?? 0) == 0 ? ErrorEmptyLevelField : Empty,
                (me.Message?.Trim()?.Length ?? 0) == 0 && !me.Properties.Any() ? ErrorNoPropsAndMessage : Empty
            }.ToImmutableArray()
                .Aggregate(Option<string[]>.None, 
                           (aggregated, x) => !IsNullOrEmpty(x) 
                                ? AppendAggregation(aggregated, x)
                                : aggregated);

        }

        public static string GetContent(this LogEvent me) =>
            me.Properties
                .Aggregate(GetPrimaryFields(me),
                           (aggregated, x) => $"{aggregated} {x.Value}");

        private static string GetPrimaryFields(LogEvent me) =>
            $"{me.Level} {me.Exception} {me.MessageTemplate} {me.Message}";
    }
}