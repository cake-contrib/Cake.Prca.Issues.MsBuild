namespace Cake.Prca.Issues.MsBuild.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using Core.Diagnostics;
    using Testing;

    public class MsBuildCodeAnalysisProviderFixture
    {
        public MsBuildCodeAnalysisProviderFixture(string fileResourceName)
        {
            this.Log = new FakeLog { Verbosity = Verbosity.Normal };

            using (var stream = this.GetType().Assembly.GetManifestResourceStream("Cake.Prca.Issues.MsBuild.Tests.Testfiles." + fileResourceName))
            {
                using (var sr = new StreamReader(stream))
                {
                    this.Settings =
                        MsBuildCodeAnalysisSettings.FromContent(
                            sr.ReadToEnd(),
                            new XmlFileLoggerFormat(this.Log));
                }
            }

            this.PrcaSettings =
                new ReportCodeAnalysisIssuesToPullRequestSettings(@"c:\Source\Cake.Prca");
        }

        public FakeLog Log { get; set; }

        public MsBuildCodeAnalysisSettings Settings { get; set; }

        public ReportCodeAnalysisIssuesToPullRequestSettings PrcaSettings { get; set; }

        public MsBuildCodeAnalysisProvider Create()
        {
            var provider = new MsBuildCodeAnalysisProvider(this.Log, this.Settings);
            provider.Initialize(this.PrcaSettings);
            return provider;
        }

        public IEnumerable<ICodeAnalysisIssue> ReadIssues()
        {
            var codeAnalysisProvider = this.Create();
            return codeAnalysisProvider.ReadIssues(PrcaCommentFormat.PlainText);
        }
    }
}
