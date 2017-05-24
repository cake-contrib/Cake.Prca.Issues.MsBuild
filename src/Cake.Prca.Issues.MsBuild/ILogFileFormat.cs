namespace Cake.Prca.Issues.MsBuild
{
    using System.Collections.Generic;

    /// <summary>
    /// Definition of a MsBuild log file format.
    /// </summary>
    public interface ILogFileFormat
    {
        /// <summary>
        /// Gets all code analysis issues.
        /// </summary>
        /// <param name="prcaSettings">General settings to use.</param>
        /// <param name="settings">Settings for code analysis provider to use.</param>
        /// <returns>List of code analysis issues</returns>
        IEnumerable<ICodeAnalysisIssue> ReadIssues(
            ReportCodeAnalysisIssuesToPullRequestSettings prcaSettings,
            MsBuildIssuesSettings settings);
    }
}
