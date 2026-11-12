namespace AiCodeReviewAgent.Models
{


    public class CodeReviewResult
    {
        public string Summary { get; set; } = "";

        public string OverallRisk { get; set; } = "Low";

        public List<ReviewFinding> Findings { get; set; } = new();

        public List<string> MissingTests { get; set; } = new();
    }

    public class ReviewFinding
    {
        public string Category { get; set; } = "";

        public string Severity { get; set; } = "";

        public string File { get; set; } = "";

        public int Line { get; set; }

        public string Description { get; set; } = "";

        public string Recommendation { get; set; } = "";
    }
}