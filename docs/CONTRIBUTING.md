# Contributing to OmoriMod

Thanks for wanting to help improve OmoriMod.

Return to the [documentation index](README.md).

## Development setup

This project includes VS Code build and launch configurations for building the mod and debugging it in tModLoader. The configurations use the `TMLSTEAMPATH` environment variable so that each contributor can use their own tModLoader installation.

From the repository root, run the setup script in PowerShell:

```powershell
.\PowerShell\setup.ps1
```

Enter the path to your tModLoader Steam installation when prompted, or press Enter to use the default Steam location. The script saves the path as a permanent user environment variable.

If PowerShell reports that script execution is disabled, run the following command once and then rerun the setup script:

```powershell
Set-ExecutionPolicy -Scope CurrentUser RemoteSigned
```

After setup, fully close and reopen VS Code so it can read the new environment variable. To build and debug the mod, open the **Run and Debug** panel (`Ctrl+Shift+D`) and select the profile matching your debugger:

- Use **Launch tModLoader — Microsoft C#** with the Microsoft C# debugger when ReSharper is disabled or not installed. This preserves the original `dotnet tModLoader.dll` workflow.
- Use **Launch tModLoader — ReSharper** when ReSharper 2026.2 or newer is enabled. ReSharper 2026.2 introduced its own `coreclr` debugger and requires `tModLoader.dll` and the bundled .NET runtime to be configured separately.

Press `F5` after selecting the profile. VS Code remembers the last selected profile, so this is normally a one-time choice. Both profiles use the same `TMLSTEAMPATH` environment variable, setup script, and pre-launch build task.

For additional setup troubleshooting, see the [Dev Setup section in the project README](../README.md#dev-setup-vs-code).

## Formatting and linting

Before opening a pull request, run the formatter from the repository root:

```powershell
dotnet format OmoriMod.csproj --include . --severity info
```

The `--include .` option limits formatting to files inside OmoriMod and prevents `dotnet format` from formatting the sibling tModLoader source tree.

To reproduce the lint check used by GitHub Actions without modifying files, run:

```powershell
dotnet restore OmoriMod.csproj
dotnet format OmoriMod.csproj --include . --no-restore --verify-no-changes --severity warn --verbosity minimal
```

The explicit restore makes `--no-restore` safe and avoids restoring the project twice. The lint command exits with a nonzero status if it finds files that would be changed.

## Workflow

1. Fork this repository.
2. Create a branch in your fork for your change.
3. Make a focused change with a clear commit message.
4. Run the formatting and linting commands above.
5. Open a pull request against `K3LV0N/OmoriMod:main`.
6. Wait for maintainer review. A code owner must approve before the PR can be merged.

Direct pushes to `main` are restricted. Pull requests are the normal path for changes.

## Pull request expectations

- Keep each PR focused on one fix or feature.
- Describe what changed and why.
- Include screenshots or clips for visible gameplay changes when helpful.
- Mention any manual testing you performed.
- Avoid unrelated formatting or cleanup in the same PR.

## Issues

Use issues for bug reports, feature ideas, and mod compatibility notes. Please include enough detail for someone else to reproduce or understand the request.
