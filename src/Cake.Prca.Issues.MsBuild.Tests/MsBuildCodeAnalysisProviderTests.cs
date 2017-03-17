namespace Cake.Prca.Issues.MsBuild.Tests
{
    using Testing;
    using Xunit;

    public class MsBuildCodeAnalysisProviderTests
    {
        public sealed class TheMsBuildCodeAnalysisProviderCtor
        {
            [Fact]
            public void Should_Throw_If_Log_Is_Null()
            {
                // Given / When
                var result = Record.Exception(() => 
                    new MsBuildCodeAnalysisProvider(
                        null, 
                        MsBuildCodeAnalysisSettings.FromContent(
                            "Foo", 
                            new XmlFileLoggerFormat(new FakeLog()),  @"c:\src")));

                // Then
                result.IsArgumentNullException("log");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                var result = Record.Exception(() =>
                    new MsBuildCodeAnalysisProvider(
                        new FakeLog(), 
                        null));

                // Then
                result.IsArgumentNullException("settings");
            }
        }
    }
}
