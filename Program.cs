using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;

namespace QuickSheetGitLog;

class Program
{
    static void Main(string[] args)
    {
        string? line;
        while ((line = Console.ReadLine()) != null)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            try
            {
                using var doc = JsonDocument.Parse(line);
                var root = doc.RootElement;
                string type = root.TryGetProperty("type", out var t) ? t.GetString() ?? "" : "";

                if (type == "init" || type == "activate")
                {
                    var resp = new { type = "status", status = "ready" };
                    Console.WriteLine(JsonSerializer.Serialize(resp));
                    Console.Out.Flush();
                }
                else if (type == "request")
                {
                    string param = "";
                    if (root.TryGetProperty("params", out var paramsEl) && paramsEl.ValueKind == JsonValueKind.Array)
                    {
                        var arr = paramsEl.EnumerateArray();
                        if (arr.MoveNext()) param = arr.Current.GetString() ?? "";
                    }
                    else if (root.TryGetProperty("arguments", out var argsEl) && argsEl.ValueKind == JsonValueKind.Array)
                    {
                        var arr = argsEl.EnumerateArray();
                        if (arr.MoveNext()) param = arr.Current.GetString() ?? "";
                    }

                    var cells = GetGitLog(param.Trim());
                    var response = new { type = "response", cells };
                    Console.WriteLine(JsonSerializer.Serialize(response));
                    Console.Out.Flush();
                }
            }
            catch
            {
                // Ignore malformed messages
            }
        }
    }

    static List<object> GetGitLog(string param)
    {
        var cells = new List<object>();

        // Parse param: could be a number (commit count) or a path, or "path count"
        int count = 10;
        string repoPath = ".";

        if (!string.IsNullOrEmpty(param))
        {
            // Try parsing as just a number
            if (int.TryParse(param, out int n) && n > 0)
            {
                count = Math.Min(n, 50);
            }
            else
            {
                // Try "path count" or just "path"
                var parts = param.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                repoPath = parts[0];
                if (parts.Length > 1 && int.TryParse(parts[1], out int n2) && n2 > 0)
                    count = Math.Min(n2, 50);
            }
        }

        // Run git log
        string gitArgs = $"log --format=%H%n%h%n%an%n%ar%n%s -n {count}";
        string? output = RunGit(gitArgs, repoPath);

        if (output == null)
        {
            cells.Add(new { r = 0, c = 0, v = "⚠ Not a git repository or git not found" });
            cells.Add(new { r = 1, c = 0, v = $"Path: {repoPath}" });
            cells.Add(new { r = 2, c = 0, v = "Usage: gitlog: [path] [count]" });
            return cells;
        }

        // Header
        cells.Add(new { r = 0, c = 0, v = "📋 GIT LOG" });
        cells.Add(new { r = 0, c = 1, v = "AUTHOR" });
        cells.Add(new { r = 0, c = 2, v = "WHEN" });
        cells.Add(new { r = 0, c = 3, v = "MESSAGE" });

        var lines = output.Split('\n', StringSplitOptions.None);
        int row = 1;

        // Each commit is 5 lines: full hash, short hash, author, relative date, subject
        for (int i = 0; i + 4 < lines.Length; i += 5)
        {
            string shortHash = lines[i + 1].Trim();
            string author = lines[i + 2].Trim();
            string when = lines[i + 3].Trim();
            string subject = lines[i + 4].Trim();

            cells.Add(new { r = row, c = 0, v = shortHash });
            cells.Add(new { r = row, c = 1, v = author });
            cells.Add(new { r = row, c = 2, v = when });
            cells.Add(new { r = row, c = 3, v = subject });
            row++;
        }

        if (row == 1)
        {
            cells.Add(new { r = 1, c = 0, v = "(no commits found)" });
        }

        // Summary row
        string branchOutput = RunGit("rev-parse --abbrev-ref HEAD", repoPath) ?? "unknown";
        cells.Add(new { r = row + 1, c = 0, v = $"Branch: {branchOutput.Trim()}" });
        cells.Add(new { r = row + 1, c = 1, v = $"Showing {row - 1} commits" });

        return cells;
    }

    static string? RunGit(string arguments, string workingDir)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = arguments,
                WorkingDirectory = workingDir,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using var proc = Process.Start(psi);
            if (proc == null) return null;
            string stdout = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit(5000);
            return proc.ExitCode == 0 ? stdout : null;
        }
        catch
        {
            return null;
        }
    }
}
