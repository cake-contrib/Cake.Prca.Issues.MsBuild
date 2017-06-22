namespace Cake.Prca.Issues.MsBuild.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using Core.Diagnostics;
    using Testing;

    internal class MsBuildIssuesProviderFixture
    {
        public MsBuildIssuesProviderFixture(string fileResourceName)
        {
            this.Log = new FakeLog { Verbosity = Verbosity.Normal };

            using (var stream = this.GetType().Assembly.GetManifestResourceStream("Cake.Prca.Issues.MsBuild.Tests.Testfiles." + fileResourceName))
            {
                using (var sr = new StreamReader(stream))
                {
                    this.Settings =
                        MsBuildIssuesSettings.FromContent(
                            sr.ReadToEnd(),
                            new XmlFileLoggerFormat(this.Log));
                }
            }

            this.PrcaSettings =
                new ReportIssuesToPullRequestSettings(@"c:\Source\Cake.Prca");
        }

        public FakeLog Log { get; set; }

        public MsBuildIssuesSettings Settings { get; set; }

        public ReportIssuesToPullRequestSettings PrcaSettings { get; set; }

        public MsBuildIssuesProvider Create()
        {
            var provider = new MsBuildIssuesProvider(this.Log, this.Settings);
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
