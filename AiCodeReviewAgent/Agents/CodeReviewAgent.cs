
using AiCodeReviewAgent.Services;
using AiCodeReviewAgent.Tools;

namespace AiCodeReviewAgent.Agents;

public class CodeReviewAgent
{
    private readonly GitDiffAnalysisTool _diffTool;
    private readonly GeminiService _geminiService;

    public CodeReviewAgent(GitDiffAnalysisTool diffTool, GeminiService geminiService)
    {
        _diffTool = diffTool;
        _geminiService = geminiService;
    }

    public async Task<object> ReviewAsync(
        string gitDiff,
        CancellationToken cancellationToken = default)
    {
        // Step 1: Validate and analyze Git diff
        var diffAnalysis = _diffTool.Analyze(gitDiff);

        // Step 2: Send Git diff and line mapping to Gemini
        var aiReview = await _geminiService.AnalyzeAsync(gitDiff, diffAnalysis, cancellationToken);

        // Step 3: Validate AI findings against actual added lines
        aiReview.Findings = aiReview.Findings
            .Where(finding =>
                !string.IsNullOrWhiteSpace(finding.File) &&
                diffAnalysis.AddedLineDetails.Any(line =>
                    string.Equals(
                        line.File.Replace('\\', '/'),
                        finding.File.Replace('\\', '/'),
                        StringComparison.OrdinalIgnoreCase)
                    && line.LineNumber == finding.Line))
            .ToList();

        // Step 4: Combine local and AI analysis
        return new
        {
            Message = "AI code review completed",
            DiffAnalysis = diffAnalysis,
            AiReview = aiReview
        };
    }
}
