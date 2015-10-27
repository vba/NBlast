using FluentAssertions;
using NBlast.Rest.Model.Write;
using System.Linq;
using Xunit;
namespace NBlast.Rest.Tests.Model.Write
{
    public class LogEventExtensionsTest
    {
        [Fact(DisplayName = "It should validate a valid event as expected")]
        public void Check_Validate_valid()
        {
            // Given
            var sut = new LogEvent("Info", items: new[] {new LogEventProperty("key", 1)});

            // When
            var actual = sut.Validate();

            // Then
            actual.IsSome.Should().BeFalse();
        }

        [Fact(DisplayName = "It should cumulate invalid cases as expected")]
        public void Check_Validate_errors_collect()
        {
            // Given
            var sut = new LogEvent("");

            // When
            var actual = sut.Validate();

            // Then
            actual.IsSome.Should().BeTrue();
            actual.Match(Some: x => x, None: () => new string[0])
                .Should().HaveCount(2)
                    .And.Contain(LogEventExtensions.ErrorNoPropsAndMessage)
                    .And.Contain(LogEventExtensions.ErrorEmptyLevelField);
        }

        [Fact(DisplayName = "It should invalidate event if neither message nor properties are available")]
        public void Check_Validate_Message_and_Props()
        {
            // Given
            var sut = new LogEvent("Info");

            // When
            var actual = sut.Validate();

            // Then
            actual.IsSome.Should().BeTrue();
            actual.Match(Some: x => x, None: () => new string[0])
                .Should().HaveCount(1).And.Contain(LogEventExtensions.ErrorNoPropsAndMessage);
        }

        [Fact(DisplayName = "It should invalidate event if level is empty")]
        public void Check_Validate_level()
        {
            // Given
            var sut = new LogEvent("", message: "Some");

            // When
            var actual = sut.Validate();

            // Then
            actual.IsSome.Should().BeTrue();
            actual.Match(Some: x => x, None: () => new string[0])
                .Should().HaveCount(1).And.Contain(LogEventExtensions.ErrorEmptyLevelField);
        }

        [Fact(DisplayName = "It should invalidate event if level contains only \\s+")]
        public void Check_Validate_level_with_whitespace()
        {
            // Given
            var sut = new LogEvent("      ", message: "Some");

            // When
            var actual = sut.Validate();

            // Then
            actual.IsSome.Should().BeTrue();
            actual.Match(Some: x => x, None: () => new string[0])
                .Should().HaveCount(1).And.Contain(LogEventExtensions.ErrorEmptyLevelField);
        }
    }
}
