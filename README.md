# quicksheet-gitlog

A [QuickSheet](https://github.com/cemheren/QuickSheet) extension that displays recent git commits in your spreadsheet cells.

See repo activity at a glance — right on your desktop or in your terminal.

## Usage

In any QuickSheet cell, type:

```
ext: gitlog:
```

This shows the last 10 commits from the current directory.

### Options

| Cell content | Description |
|---|---|
| `ext: gitlog:` | Last 10 commits in current dir |
| `ext: gitlog: 20` | Last 20 commits |
| `ext: gitlog: /path/to/repo` | Commits from a specific repo |
| `ext: gitlog: /path/to/repo 5` | Last 5 commits from a specific repo |

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

## Install

1. Clone this repo next to your QuickSheet installation
2. QuickSheet auto-discovers extensions in sibling directories

Or add manually to your extensions config:

```json
{
  "name": "quicksheet-gitlog",
  "entry": "dotnet run --project /path/to/quicksheet-gitlog"
}
```

## Requirements

- .NET 9.0 SDK
- `git` available on PATH
- [QuickSheet](https://github.com/cemheren/QuickSheet) v0.10.0+

## Zero dependencies

No NuGet packages. Uses `System.Diagnostics.Process` to call git and `System.Text.Json` (built into .NET) for the JSON-lines protocol.

## License

MIT
