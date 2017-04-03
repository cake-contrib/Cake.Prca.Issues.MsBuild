namespace Cake.Prca.Issues.MsBuild.Tests
{
    using Shouldly;
    using Xunit;

    public class MsBuildRuleUrlResolverTests
    {
        public sealed class TheResolveRuleUrlMethod
        {
            [Fact]
            public void Should_Throw_If_Rule_Is_Null()
            {
                // Given / When
                var result = Record.Exception(() => new MsBuildRuleUrlResolver().ResolveRuleUrl(null));

                // Then
                result.IsArgumentNullException("rule");
            }

            [Fact]
            public void Should_Throw_If_Rule_Is_Empty()
            {
                // Given / When
                var result = Record.Exception(() => new MsBuildRuleUrlResolver().ResolveRuleUrl(string.Empty));

                // Then
                result.IsArgumentOutOfRangeException("rule");
            }

            [Fact]
            public void Should_Throw_If_Rule_Is_WhiteSpace()
            {
                // Given / When
                var result = Record.Exception(() => new MsBuildRuleUrlResolver().ResolveRuleUrl(" "));

                // Then
                result.IsArgumentOutOfRangeException("rule");
            }

            [Theory]
            [InlineData("CA2201", "https://www.google.im/search?q=\"CA2201:\"+site:msdn.microsoft.com")]
            [InlineData("SA1652", "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/SA1652.md")]
            public void Should_Resolve_Url(string rule, string expectedUrl)
            {
                // Given
                var urlResolver = new MsBuildRuleUrlResolver();

                // When
                var ruleUrl = urlResolver.ResolveRuleUrl(rule);

                // Then
                ruleUrl.ToString().ShouldBe(expectedUrl);
            }

            [Theory]
            [InlineData("CA")]
            [InlineData("2201")]
            [InlineData("CA2201Foo")]
            [InlineData("CS0219")]
            public void Should_Return_Null_For_Unknown_Rules(string rule)
            {
                // Given
                var urlResolver = new MsBuildRuleUrlResolver();

                // When
                var ruleUrl = urlResolver.ResolveRuleUrl(rule);

                // Then
                ruleUrl.ShouldBeNull();
            }
        }
    }
}
