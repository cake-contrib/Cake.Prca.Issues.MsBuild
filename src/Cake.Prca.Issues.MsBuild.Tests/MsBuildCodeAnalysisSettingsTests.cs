namespace Cake.Prca.Issues.MsBuild.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Core.IO;
    using Shouldly;
    using Testing;
    using Xunit;

    public class MsBuildCodeAnalysisSettingsTests
    {
        public sealed class TheMsBuildCodeAnalysisSettings
        {
            [Fact]
            public void Should_Throw_If_LogFilePath_Is_Null()
            {
                // Given / When
                var result = Record.Exception(() =>
                    MsBuildCodeAnalysisSettings.FromFilePath(
                        null,
                        new XmlFileLoggerFormat(new FakeLog()),
                        new DirectoryPath(@"C:\")));

                // Then
                result.IsArgumentNullException("logFilePath");
            }

            [Fact]
            public void Should_Throw_If_Format_For_LogFilePath_Is_Null()
            {
                // Given / When
                var result = Record.Exception(() =>
                    MsBuildCodeAnalysisSettings.FromFilePath(
                        @"C:\foo.log",
                        null,
                        new DirectoryPath(@"C:\")));

                // Then
                result.IsArgumentNullException("format");
            }

            [Fact]
            public void Should_Throw_If_RepositoryRoot_For_LogFilePath_Is_Null()
            {
                // Given / When
                var result = Record.Exception(() =>
                    MsBuildCodeAnalysisSettings.FromFilePath(
                        @"C:\foo.log",
                        new XmlFileLoggerFormat(new FakeLog()),
                        null));

                // Then
                result.IsArgumentNullException("repositoryRoot");
            }

            [Fact]
            public void Should_Throw_If_LogFileContent_Is_Null()
            {
                // Given / When
                var result = Record.Exception(() =>
                    MsBuildCodeAnalysisSettings.FromContent(
                        null,
                        new XmlFileLoggerFormat(new FakeLog()),
                        new DirectoryPath(@"C:\")));

                // Then
                result.IsArgumentNullException("logFileContent");
            }

            [Fact]
            public void Should_Throw_If_LogFileContent_Is_Empty()
            {
                // Given / When
                var result = Record.Exception(() =>
                    MsBuildCodeAnalysisSettings.FromContent(
                        string.Empty,
                        new XmlFileLoggerFormat(new FakeLog()),
                        new DirectoryPath(@"C:\")));

                // Then
                result.IsArgumentOutOfRangeException("logFileContent");
            }

            [Fact]
            public void Should_Throw_If_LogFileContent_Is_WhiteSpace()
            {
                // Given / When
                var result = Record.Exception(() =>
                    MsBuildCodeAnalysisSettings.FromContent(
                        " ",
                        new XmlFileLoggerFormat(new FakeLog()),
                        new DirectoryPath(@"C:\")));

                // Then
                result.IsArgumentOutOfRangeException("logFileContent");
            }

            [Fact]
            public void Should_Throw_If_Format_For_LogFileContent_Is_Null()
            {
                // Given / When
                var result = Record.Exception(() =>
                    MsBuildCodeAnalysisSettings.FromContent(
                        "foo",
                        null,
                        new DirectoryPath(@"C:\")));

                // Then
                result.IsArgumentNullException("format");
            }

            [Fact]
            public void Should_Throw_If_RepositoryRoot_For_LogFileContent_Is_Null()
            {
                // Given / When
                var result = Record.Exception(() =>
                    MsBuildCodeAnalysisSettings.FromContent(
                        "foo",
                        new XmlFileLoggerFormat(new FakeLog()),
                        null));

                // Then
                result.IsArgumentNullException("repositoryRoot");
            }

            [Fact]
            public void Should_Set_Property_Values_Passed_To_Constructor()
            {
                // Given
                const string logFileContent = "foo";
                var format = new XmlFileLoggerFormat(new FakeLog());
                var repoRoot = new DirectoryPath(@"C:\");

                // When
                var settings = MsBuildCodeAnalysisSettings.FromContent(logFileContent, format, repoRoot);

                // Then
                settings.LogFileContent.ShouldBe(logFileContent);
                settings.Format.ShouldBe(format);
                settings.RepositoryRoot.ShouldBe(repoRoot);
            }

            [Fact]
            public void Should_Read_File_From_Disk()
            {
                var fileName = System.IO.Path.GetTempFileName();
                try
                {
                    // Given
                    string expected;
                    using (var ms = new MemoryStream())
                    using (var stream = this.GetType().Assembly.GetManifestResourceStream("Cake.Prca.Issues.MsBuild.Tests.Testfiles.IssueWithFile.xml"))
                    {
                        stream.CopyTo(ms);
                        var data = ms.ToArray();

                        using (var file = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                        {
                            file.Write(data, 0, data.Length);
                        }

                        expected = ConvertFromUtf8(data);
                    }

                    // When
                    var settings =
                        MsBuildCodeAnalysisSettings.FromFilePath(
                            fileName,
                            new XmlFileLoggerFormat(new FakeLog()),
                            new DirectoryPath(@"C:\"));

                    // Then
                    settings.LogFileContent.ShouldBe(expected);
                }
                finally
                {
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }
                }
            }

            private static string ConvertFromUtf8(byte[] bytes)
            {
                var enc = new UTF8Encoding(true);
                var preamble = enc.GetPreamble();

                if (preamble.Where((p, i) => p != bytes[i]).Any())
                {
                    throw new ArgumentException("Not utf8-BOM");
                }

                return enc.GetString(bytes.Skip(preamble.Length).ToArray());
            }
        }
    }
}
