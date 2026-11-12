using AiCodeReviewAgent.Agents;
using AiCodeReviewAgent.Models;
using Microsoft.AspNetCore.Mvc;

namespace AiCodeReviewAgent.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly CodeReviewAgent _agent;
    private readonly ILogger<ReviewController> _logger;

    public ReviewController(
        CodeReviewAgent agent,
        ILogger<ReviewController> logger)
    {
        _agent = agent;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> ReviewCode(
        [FromBody] ReviewRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request?.GitDiff))
            return BadRequest("Git diff cannot be empty.");

        if (request.GitDiff.Length > 50000)
            return BadRequest("Git diff is too large.");

        try
        {
            var result = await _agent.ReviewAsync(request.GitDiff,
                cancellationToken);

            return Ok(result);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "AI provider request failed");

            return StatusCode(502, new
            {
                Message = "AI review provider unavailable."
            });
        }
        catch (OperationCanceledException)
            when (!cancellationToken.IsCancellationRequested)
        {
            return StatusCode(504, new
            {
                Message = "AI review timed out."
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AI review failed");

            return StatusCode(500, new
            {
                Message = "AI review failed. Check server logs."
            });
        }
    }
}
//using AiCodeReviewAgent.Models;
//using AiCodeReviewAgent.Tools;
//using Microsoft.AspNetCore.Mvc;

//namespace AiCodeReviewAgent.Controllers;

//[ApiController]
//[Route("api/[controller]")]
//public class ReviewController : ControllerBase
//{
//    //[HttpPost]
//    //public IActionResult ReviewCode([FromBody] ReviewRequest request)
//    //{
//    //    if (string.IsNullOrWhiteSpace(request.GitDiff))
//    //    {
//    //        return BadRequest("Git diff cannot be empty.");
//    //    }

//    //    return Ok(new
//    //    {
//    //        Message = "Git diff received successfully!",
//    //        CharactersReceived = request.GitDiff.Length,
//    //        Status = "Ready for AI review"
//    //    });
//    //}
//    private readonly GitDiffAnalysisTool _diffTool;

//    public ReviewController(GitDiffAnalysisTool diffTool)
//    {
//        _diffTool = diffTool;
//    }

//    [HttpPost]
//    public IActionResult ReviewCode(ReviewRequest request)
//    {
//        if (string.IsNullOrWhiteSpace(request?.GitDiff))
//            return BadRequest("Git diff cannot be empty.");

//        var result = _diffTool.Analyze(request.GitDiff);

//        return Ok(new
//        {
//            Message = "Git diff analyzed successfully",
//            Analysis = result
//        });
//    }
//}