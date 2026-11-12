\# AI Code Review Agent



\## 1. What it does



An AI-powered code review agent that analyzes Git diffs and identifies potential bugs, security vulnerabilities, performance issues, maintainability problems, and missing test cases.



It returns structured JSON feedback containing:

\- Modified files and added/deleted lines.

\- Code review findings and severity.

\- Overall risk assessment.

\- Recommended fixes.

\- Missing test cases.



\## 2. How it was built



\*\*Technology stack:\*\*

\- Backend: ASP.NET Core Web API (.NET 10)

\- Language: C#

\- AI Model: Google Gemini API

\- API Documentation: Swagger

\- HTTP Communication: HttpClient



\*\*Architecture:\*\*

1\. ReviewController receives the Git diff.

2\. CodeReviewAgent coordinates the review process.

3\. GitDiffAnalysisTool extracts modified files, added lines, and deleted lines.

4\. GeminiService sends the diff and analysis context to Gemini.

5\. Gemini returns structured JSON feedback.

6\. The agent combines the results and returns the response.



\## 3. Challenges and how they were handled



\- OpenAI API quota and billing limitations: switched to Google Gemini.

\- Gemini HTTP 503 errors: implemented retry logic with exponential backoff.

\- Git diff line-number extraction: implemented local diff parsing.

\- AI response parsing: used System.Text.Json to deserialize structured JSON.

\- API key configuration: used configuration-based secret management.



\## 4. Future improvements



\- Integrate GitHub Pull Request APIs.

\- Automatically fetch pull request diffs.

\- Add automated unit tests.

\- Support multiple AI providers.

\- Add CI/CD integration.

\- Generate downloadable code review reports.



\## 5. Running the application



1\. Configure the Gemini API key using .NET User Secrets.

2\. Configure the Gemini model and BaseUrl in appsettings.json.

3\. Run the ASP.NET Core application.

4\. Open Swagger and submit a Git diff to the Review endpoint.



\*\*Note:\*\* Never commit API keys or other credentials to GitHub.

