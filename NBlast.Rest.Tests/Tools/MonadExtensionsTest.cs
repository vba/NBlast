using System;
using FluentAssertions;
using NBlast.Rest.Tools;
using Xunit;

namespace NBlast.Rest.Tests.Tools
{
    public class MonadExtensionsTest
    {
        [Fact(DisplayName = "It should get out on first true condition")]
        public void Check_If_extension()
        {
            // given
            var exp1 = "first";
            var exp2 = "second";
            var exp3 = "third";

            // when
            var actual = new object().Bind()
                .If(_ => true,
                    @true: x => "first",
                    @false: x => "first"
                ).Value;
//                .If(_ => false)
//                .With(s => exp3)
                ;

            // then
            actual.Should().Be(exp1);
        } 
    }
}