namespace Cake.Prca.Issues.MsBuild
{
    using System;
    using System.Text;

    /// <summary>
    /// Class for retrieving an URL linking to a site describing a rule.
    /// </summary>
    internal static class RuleUrlResolver
    {
        /// <summary>
        /// Returns an URL linking to a site describing a specific rule.
        /// </summary>
        /// <param name="rule">Code of the rule for which the URL should be retrieved.</param>
        /// <returns>URL linking to a site describing the rule, or <c>null</c> if <paramref name="rule"/>
        /// could not be parsed.</returns>
        public static Uri ResolveRuleUrl(string rule)
        {
            rule.NotNullOrWhiteSpace(nameof(rule));

            string category;
            int ruleId;
            if (!TryGetRuleCategory(rule, out category, out ruleId))
            {
                return null;
            }

            switch (category.ToLowerInvariant())
            {
                case "ca":
                    return new Uri("https://www.google.im/search?q=%22" + rule + ":%22+site:msdn.microsoft.com");
                case "sa":
                    return new Uri("https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/" + rule + ".md");
            }

            return null;
        }

        /// <summary>
        /// Parses a rule into its category and ID.
        /// </summary>
        /// <param name="rule">Rule which should be parsed.</param>
        /// <param name="category">Category of the rule.</param>
        /// <param name="ruleId">Id of the rule.</param>
        /// <returns><c>true</c> if rule could by parsed successfully, otherwise <c>false</c>.</returns>
        private static bool TryGetRuleCategory(string rule, out string category, out int ruleId)
        {
            category = string.Empty;
            ruleId = 0;

            // Parse the rule. Expect it in the form starting with a identifier containing characters
            // followed by the rule id as a number.
            var digitIndex = -1;
            var categoryBuilder = new StringBuilder();
            for (var index = 0; index < rule.Length; index++)
            {
                var currentChar = rule[index];
                if (char.IsDigit(currentChar))
                {
                    digitIndex = index;
                    break;
                }

                categoryBuilder.Append(currentChar);
            }

            // If rule doesn't contain numbers return false.
            if (digitIndex < 0)
            {
                return false;
            }

            // Try to parse the part after the first number to an integer.
            if (!int.TryParse(rule.Substring(digitIndex), out ruleId))
            {
                return false;
            }

            category = categoryBuilder.ToString();
            return true;
        }
    }
}
