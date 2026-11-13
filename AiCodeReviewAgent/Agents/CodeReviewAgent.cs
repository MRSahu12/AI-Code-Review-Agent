
using AiCodeReviewAgent.Services;
using AiCodeReviewAgent.Tools;

namespace AiCodeReviewAgent.Agents;

public class CodeReviewAgent
{
    private readonly GitDiffAnalysisTool _diffTool;
    private readonly SecurityAnalysisTool _securityTool;
    private readonly TestAnalysisTool _testTool;
    private readonly GeminiService _geminiService;

    public CodeReviewAgent(GitDiffAnalysisTool diffTool, SecurityAnalysisTool securityTool, TestAnalysisTool testTool, GeminiService geminiService)
    {
        _diffTool = diffTool;
        _securityTool = securityTool;
        _testTool = testTool;
        _geminiService = geminiService;
    }

    public async Task<object> ReviewAsync(string gitDiff, CancellationToken cancellationToken = default)
    {
        // 1. Analyze Git diff
        var diffAnalysis = _diffTool.Analyze(gitDiff);

        // 2. Run security analysis
        var securityFindings = _securityTool.Analyze(diffAnalysis);

        // 3. Run test analysis
        var testFindings = _testTool.Analyze(diffAnalysis);

        // 4. Combine local tool findings
        var toolFindings = securityFindings.Concat(testFindings).ToList();

        // 5. Send diff and tool findings to Gemini
        var aiReview = await _geminiService.AnalyzeAsync(gitDiff, diffAnalysis, toolFindings, cancellationToken);

        // 6. Return review response
        return new
        {
            Message = "AI code review completed",
            DiffAnalysis = diffAnalysis,
            ToolAnalysis = new
            {
                SecurityFindings = securityFindings,
                TestFindings = testFindings
            },
            AiReview = aiReview
        };
    }
}
