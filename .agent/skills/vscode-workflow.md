# Skill: vscode-workflow

## Zweck

Nutze diesen Skill für VS Code im Projekt `SudokuMoveAdvisor`.

## Standardbefehle

- Restore: `dotnet restore`
- Build: `dotnet build`
- Run: `dotnet run --project src/VibeConsoleApp`
- Test: `dotnet test`

## Hinweise

- Beispielrätsel liegt in `appsettings.json`.
- Für ad-hoc Tests kann `--puzzle=` verwendet werden.
- Debugging läuft über `.vscode/launch.json` gegen `src/VibeConsoleApp`.

## Done-Kriterien

- Projekt baut lokal in VS Code
- Beispielrätsel startet ohne Zusatzkonfiguration
- Testlauf ist über Terminal oder Task möglich
