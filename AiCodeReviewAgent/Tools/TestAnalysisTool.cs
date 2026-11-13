
namespace AiCodeReviewAgent.Tools;

public class TestAnalysisTool
{
    public List<string> Analyze(DiffAnalysisResult diffAnalysis)
    {
        var findings = new List<string>();

        bool containsTestFile = diffAnalysis.ModifiedFiles.Any(
            file =>
                file.Contains("/Tests/", StringComparison.OrdinalIgnoreCase) || file.Contains("\\Tests\\", StringComparison.OrdinalIgnoreCase)
                || file.EndsWith("Tests.cs", StringComparison.OrdinalIgnoreCase) || file.EndsWith("Test.cs", StringComparison.OrdinalIgnoreCase)
                || file.EndsWith(".Tests.csproj", StringComparison.OrdinalIgnoreCase));

        bool containsProductionCode = diffAnalysis.ModifiedFiles.Any(file => file.EndsWith(".cs", StringComparison.OrdinalIgnoreCase)
                    && !file.EndsWith("Tests.cs", StringComparison.OrdinalIgnoreCase)
                    && !file.EndsWith("Test.cs", StringComparison.OrdinalIgnoreCase));

        if (containsProductionCode && !containsTestFile)
        {
            findings.Add("No test files were included in this diff. " + "Verify whether existing unit and integration tests " +
                "cover the changed functionality.");
        }

        return findings;
    }
}
