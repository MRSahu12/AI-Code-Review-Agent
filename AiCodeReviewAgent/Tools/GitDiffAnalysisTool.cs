
using System.Globalization;
using System.Text.RegularExpressions;

namespace AiCodeReviewAgent.Tools;

public class GitDiffAnalysisTool
{
    private static readonly Regex HunkHeaderRegex = new(@"^@@ -\d+(?:,\d+)? \+(\d+)(?:,\d+)? @@", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public DiffAnalysisResult Analyze(string gitDiff)
    {
        if (string.IsNullOrWhiteSpace(gitDiff))
            throw new ArgumentException("Git diff cannot be empty.", nameof(gitDiff));

        var lines = gitDiff
            .Replace("\r\n", "\n")
            .Replace('\r', '\n')
            .Split('\n');

        var result = new DiffAnalysisResult();

        string? currentFile = null;
        string? oldFile = null;

        int newLineNumber = 0;
        bool insideHunk = false;

        foreach (var line in lines)
        {
            // Detect the beginning of another file's diff.
            if (line.StartsWith("diff --git ", StringComparison.Ordinal))
            {
                insideHunk = false;
                currentFile = null;
                oldFile = null;
                continue;
            }

            // Read file headers only outside a hunk.
            if (!insideHunk)
            {
                if (line.StartsWith("--- ", StringComparison.Ordinal))
                {
                    oldFile = GetDiffPath(line.Substring(4));
                    continue;
                }

                if (line.StartsWith("+++ ", StringComparison.Ordinal))
                {
                    var newFile = GetDiffPath(line.Substring(4));

                    // For deleted files, retain the old file path.
                    currentFile = newFile ?? oldFile;

                    if (!string.IsNullOrWhiteSpace(currentFile) &&
                        !result.ModifiedFiles.Contains(currentFile))
                    {
                        result.ModifiedFiles.Add(currentFile);
                    }

                    continue;
                }
            }

            // Read Git hunk header.
            if (line.StartsWith("@@", StringComparison.Ordinal))
            {
                var match = HunkHeaderRegex.Match(line);

                if (match.Success &&
                    int.TryParse(
                        match.Groups[1].Value,
                        CultureInfo.InvariantCulture,
                        out var startLine))
                {
                    newLineNumber = startLine;
                    insideHunk = !string.IsNullOrWhiteSpace(currentFile);
                }
                else
                {
                    insideHunk = false;
                }

                continue;
            }

            if (!insideHunk || string.IsNullOrWhiteSpace(currentFile))
                continue;

            // Ignore Git's no-newline marker.
            if (line.StartsWith("\\", StringComparison.Ordinal))
                continue;

            // Added line.
            if (line.StartsWith("+", StringComparison.Ordinal))
            {
                var content = line.Substring(1);

                result.AddedLines++;

                result.AddedLineDetails.Add(new ChangedLine
                {
                    File = currentFile,
                    LineNumber = newLineNumber,
                    Content = content
                });

                newLineNumber++;
            }
            // Deleted line.
            else if (line.StartsWith("-", StringComparison.Ordinal))
            {
                result.DeletedLines++;
            }
            // Unchanged context line.
            else if (line.StartsWith(" ", StringComparison.Ordinal))
            {
                newLineNumber++;
            }
        }

        return result;
    }

    private static string? GetDiffPath(string path)
    {
        path = path.Trim();

        if (path == "/dev/null")
            return null;

        if (path.StartsWith("a/", StringComparison.Ordinal) ||
            path.StartsWith("b/", StringComparison.Ordinal))
        {
            path = path.Substring(2);
        }

        return string.IsNullOrWhiteSpace(path) ? null : path;
    }
}

public class DiffAnalysisResult
{
    public List<string> ModifiedFiles { get; set; } = new();
    public int AddedLines { get; set; }
    public int DeletedLines { get; set; }
    public List<ChangedLine> AddedLineDetails { get; set; } = new();
}

public class ChangedLine
{
    public string File { get; set; } = "";
    public int LineNumber { get; set; }
    public string Content { get; set; } = "";
}
