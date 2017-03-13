namespace Cake.Prca.Issues.MsBuild.Tests
{
    using System.Linq;
    using Core.IO;
    using Shouldly;
    using Xunit;

    public class XmlFileLoggerFormatTests
    {
        public sealed class TheXmlFileLoggerFormat
        {
            [Fact]
            public void Should_Throw_If_Log_Is_Null()
            {
                // Given / When
                var result = Record.Exception(() => new XmlFileLoggerFormat(null));

                // Then
                result.IsArgumentNullException("log");
            }

            [Fact]
            public void Should_Read_Issue_With_File_Correct()
            {
                // Given
                var fixture = new MsBuildCodeAnalysisProviderFixture("IssueWithFile.xml");

                // When
                var issues = fixture.ReadIssues();

                // Then
                issues.Count().ShouldBe(1);
                var issue = issues.Single();
                issue.AffectedFileRelativePath.ToString().ShouldBe(new FilePath(@"\src\Cake.Prca.CodeAnalysisProvider.MsBuild.Tests\MsBuildCodeAnalysisProviderTests.cs").ToString()) ;
                issue.Line.ShouldBe(1311);
                issue.Rule.ShouldBe("CA2201");
                issue.Priority.ShouldBe(0);
                issue.Message.ShouldBe(@"Microsoft.Usage : 'ConfigurationManager.GetSortedConfigFiles(String)' creates an exception of type 'ApplicationException', an exception type that is not sufficiently specific and should never be raised by user code. If this exception instance might be thrown, use a different exception type.");
            }

            [Fact]
            public void Should_Read_Issue_With_File_Without_Path_Correct()
            {
                // Given
                var fixture = new MsBuildCodeAnalysisProviderFixture("IssueWithOnlyFileName.xml");

                // When
                var issues = fixture.ReadIssues();

                // Then
                issues.Count().ShouldBe(1);
                var issue = issues.Single();
                issue.AffectedFileRelativePath.ToString().ShouldBe(new FilePath(@"\src\Cake.Prca.CodeAnalysisProvider.MsBuild.Tests\MsBuildCodeAnalysisProviderTests.cs").ToString());
                issue.Line.ShouldBe(13);
                issue.Rule.ShouldBe("CS0219");
                issue.Priority.ShouldBe(0);
                issue.Message.ShouldBe(@"The variable 'foo' is assigned but its value is never used");
            }

            [Fact]
            public void Should_Read_Issue_Without_File_Correct()
            {
                // Given
                var fixture = new MsBuildCodeAnalysisProviderFixture("IssueWithoutFile.xml");

                // When
                var issues = fixture.ReadIssues();

                // Then
                // TODO Is this correct? Or should we return them here and have the core logic or pull request system implementation taking care how to handle them?
                issues.Count().ShouldBe(0);
            }
        }
    }
}
