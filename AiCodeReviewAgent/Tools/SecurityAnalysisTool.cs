
using System.Text.RegularExpressions;

namespace AiCodeReviewAgent.Tools;

public class SecurityAnalysisTool
{
    public List<string> Analyze(DiffAnalysisResult diffAnalysis)
    {
        var findings = new List<string>();

        var addedCode = string.Join(Environment.NewLine, diffAnalysis.AddedLineDetails.Select(x => x.Content));

        // Check for raw SQL execution combined with
        // string concatenation or interpolation.
        bool usesRawSql = Regex.IsMatch(addedCode, @"\b(ExecuteSqlRaw|FromSqlRaw)\s*\(", RegexOptions.IgnoreCase);

        bool usesStringConcatenation = Regex.IsMatch(addedCode, @"\+\s*\w+") || Regex.IsMatch(addedCode, @"\$\s*""");

        if (usesRawSql && usesStringConcatenation)
        {
            findings.Add("Potential SQL injection: raw SQL execution " + "appears to be combined with string concatenation " +
                "or interpolation. Verify parameterization.");
        }

        // Check for hardcoded credentials.
        bool containsHardcodedSecret = Regex.IsMatch(addedCode, @"(?i)\b(password|secret|apiKey)\s*=\s*""[^""]+""");

        if (containsHardcodedSecret)
        {
            findings.Add("Potential hardcoded credential or secret detected. " + "Use secure configuration or a secret manager.");
        }

        return findings;
    }
}
