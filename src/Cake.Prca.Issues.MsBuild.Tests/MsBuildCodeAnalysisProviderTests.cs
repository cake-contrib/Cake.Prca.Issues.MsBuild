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
                            new XmlFileLoggerFormat(new FakeLog()))));

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

        public sealed class TheReadIssuesMethod
        {
            [Fact]
            public void Should_Throw_If_PrcaSettings_Is_Null()
            {
                // Given
                var log = new FakeLog();
                var provider =
                    new MsBuildCodeAnalysisProvider(
                        log,
                        MsBuildCodeAnalysisSettings.FromContent("Foo", new XmlFileLoggerFormat(log)));

                // When
                var result = Record.Exception(() => provider.ReadIssues(PrcaCommentFormat.PlainText));

                // Then
                result.IsInvalidOperationException("Initialize needs to be called first.");
            }
        }
    }
}
