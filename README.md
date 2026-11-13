# AI Code Review Agent

## 1. Overview

AI Code Review Agent is an AI-powered code review application built using **C#, ASP.NET Core Web API, and Google Gemini API**.

It analyzes Git diffs and identifies potential bugs, security vulnerabilities, missing test coverage, and code quality issues. The application combines rule-based static analysis tools with AI-powered reasoning to generate structured code review feedback.

## 2. Technology Stack

- **Backend:** C#, ASP.NET Core Web API
- **AI Model:** Google Gemini API
- **API Testing:** Swagger / OpenAPI
- **Code Analysis:** Custom Git Diff Analysis Tool
- **Security Analysis:** Rule-based security checks
- **Test Analysis:** Changed-file test detection
- **Version Control:** Git and GitHub

## 3. Architecture and Workflow

The application follows a tool-assisted AI agent approach.

1. **GitDiffAnalysisTool:** Parses the Git diff and identifies modified files, added/deleted lines, and changed line numbers.
2. **SecurityAnalysisTool:** Checks for potential security issues, including SQL injection patterns and hardcoded credentials.
3. **TestAnalysisTool:** Checks whether test files are included in the submitted changes and highlights potential testing gaps.
4. **GeminiService:** Sends the Git diff and local analysis findings to Google Gemini for deeper code review.
5. **CodeReviewAgent:** Orchestrates the analysis tools and AI service and combines their results.
6. **Review API:** Returns structured JSON containing the summary, overall risk, findings, and missing test recommendations.

## 4. Key Features

- Automated Git diff analysis.
- Potential bug and security vulnerability detection.
- Hardcoded credential detection.
- Missing test coverage recommendations.
- AI-generated code review feedback.
- File and line-specific findings.
- Structured JSON response with severity levels.
- Swagger-based API testing.

## 5. How to Run

**Prerequisites:**
- .NET SDK
- Google Gemini API key
- Visual Studio or VS Code

**Steps:**

1. Clone the repository:

   ```bash
   git clone https://github.com/MRSahu12/AI-Code-Review-Agent.git
   ```

2. Navigate to the project directory:

   ```bash
   cd AiCodeReviewAgent
   ```

3. Configure your Gemini API key using .NET User Secrets:

   ```bash
   dotnet user-secrets init
   dotnet user-secrets set "Gemini:ApiKey" "YOUR_GEMINI_API_KEY"
   ```

4. Configure the Gemini model and API base URL in application settings.

5. Run the application:

   ```bash
   dotnet restore
   dotnet build
   dotnet run
   ```

6. Open the Swagger URL displayed by the application and submit a Git diff using the review endpoint.

## 6. Challenges and How They Were Handled

- **Git diff parsing:** Implemented line-level parsing to identify added lines and associate AI findings with changed source locations.
- **AI response consistency:** Used structured JSON output instructions to obtain predictable review results.
- **Security analysis:** Combined local rule-based checks with Gemini analysis to identify potential vulnerabilities.
- **False positives:** AI-generated findings are treated as recommendations and require developer validation.
- **API reliability:** Added retry handling for transient Gemini API failures and appropriate response parsing.

## 7. Future Enhancements

- GitHub Pull Request integration for automated reviews.
- Support for multiple programming languages.
- More comprehensive static code analysis.
- Unit test execution and coverage analysis.
- Finding deduplication and confidence scoring.
- CI/CD pipeline integration.
- Authentication, authorization, and audit logging.

## 8. Repository

GitHub: https://github.com/MRSahu12/AI-Code-Review-Agent

**Note:** This is a prototype demonstrating an AI-assisted, tool-based code review workflow. Local security and test checks are heuristic checks, not a replacement for comprehensive static analysis, automated test execution, or human code review.
