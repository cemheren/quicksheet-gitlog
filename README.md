# quicksheet-gitlog

A [QuickSheet](https://github.com/cemheren/QuickSheet) extension that displays recent git commits in your spreadsheet cells.

See repo activity at a glance — right on your desktop or in your terminal.

## Install

In any QuickSheet cell, type:

```
ext: github:cemheren/quicksheet-gitlog
```

QuickSheet clones the repo, reads the manifest, and registers the `gitlog` prefix.

## Usage

In any cell, type:

```
gitlog: /path/to/repo,4,5
```

The format is `gitlog: <path-or-count>,<columns>,<rows>` where the last two numbers define the output grid dimensions.

### Examples

| Cell content | Description |
|---|---|
| `gitlog: .,4,11` | Last 10 commits in current dir (4 columns × 11 rows) |
| `gitlog: 20,4,21` | Last 20 commits (4 columns × 21 rows) |
| `gitlog: /path/to/repo,4,11` | Commits from a specific repo |
| `gitlog: /path/to/repo 5,4,6` | Last 5 commits from a specific repo |

> **Note:** The last two comma-separated values are always the output grid dimensions (columns, rows). QuickSheet uses these to know how much space the extension output occupies.

## Output

| Column | Content |
|---|---|
| A | Short commit hash |
| B | Author name |
| C | Relative time (e.g., "2 hours ago") |
| D | Commit message |

Plus a summary row showing current branch and commit count.

## Screenshot

```
┌──────────┬──────────┬──────────────┬────────────────────────────────┐
│ 📋 GIT LOG │ AUTHOR   │ WHEN         │ MESSAGE                        │
├──────────┼──────────┼──────────────┼────────────────────────────────┤
│ f2f3b54  │ cemheren │ 2 hours ago  │ fix: color prefix on Windows   │
│ b7dd957  │ copilot  │ 3 hours ago  │ feat: add desktop shortcuts    │
│ 4b96bb6  │ copilot  │ 5 hours ago  │ feat: theme presets (Ctrl+T)   │
│ a4cafc0  │ cemheren │ 1 day ago    │ docs: add pomodoro to README   │
├──────────┼──────────┼──────────────┼────────────────────────────────┤
│ Branch: main │ Showing 4 commits │            │                        │
└──────────┴──────────┴──────────────┴────────────────────────────────┘
```

## Requirements

- .NET 9.0 SDK
- `git` available on PATH
- [QuickSheet](https://github.com/cemheren/QuickSheet) v0.10.0+

## Zero dependencies

No NuGet packages. Uses `System.Diagnostics.Process` to call git and `System.Text.Json` (built into .NET) for the JSON-lines protocol.

## License

MIT
