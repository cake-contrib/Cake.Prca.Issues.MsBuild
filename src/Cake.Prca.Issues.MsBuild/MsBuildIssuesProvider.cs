namespace Cake.Prca.Issues.MsBuild
{
    using System.Collections.Generic;
    using Core.Diagnostics;

    /// <summary>
    /// Provider for code analysis issues reported as MsBuild warnings.
    /// </summary>
    internal class MsBuildIssuesProvider : CodeAnalysisProvider
    {
        private readonly MsBuildIssuesSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="MsBuildIssuesProvider"/> class.
        /// </summary>
        /// <param name="log">The Cake log context.</param>
        /// <param name="settings">Settings for reading the log file.</param>
        public MsBuildIssuesProvider(ICakeLog log, MsBuildIssuesSettings settings)
            : base(log)
        {
            settings.NotNull(nameof(settings));

            this.settings = settings;
        }

        /// <inheritdoc />
        protected override IEnumerable<ICodeAnalysisIssue> InternalReadIssues(PrcaCommentFormat format)
        {
            return this.settings.Format.ReadIssues(this.PrcaSettings, this.settings);
        }
    }
}
