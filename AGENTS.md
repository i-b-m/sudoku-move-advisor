# AGENTS.md

## Mission

Du arbeitest an `SudokuMoveAdvisor`, einer einfachen C#-Konsolenanwendung für VS Code. Das Ziel ist nicht ein beliebiger Sudoku-Solver, sondern ein Werkzeug, das den **nächsten plausiblen Spielzug** findet und für Menschen verständlich begründet.

## Projektkontext

- Projekttyp: einfache C# Console App
- IDE: VS Code
- Primäre Sprache: C#
- Framework: .NET 8
- Konfiguration: `appsettings.json`
- Logging: `Microsoft.Extensions.Logging`
- Testframework: xUnit
- Domänenfokus: Sudoku-Analyse mit erklärbaren Strategien

## Fachliches Ziel

Die App soll genau einen nächsten Zug vorschlagen:

- Zielzelle identifizieren
- Zahl einsetzen
- Strategie benennen
- Begründung knapp und verständlich ausgeben
- bei Reduktionsstrategien die nötigen Zwischenschritte nennen

Priorität hat **Erklärbarkeit vor Vollständigkeit**.

## Unterstützte Strategien

Direkt:

- Naked Single
- Hidden Single in Zeile
- Hidden Single in Spalte
- Hidden Single in Box

Indirekt mit Kandidatenreduktion:

- Pointing Pair / Triple
- Box/Line Reduction
- Naked Pair
- Hidden Pair
- X-Wing
- Swordfish

## Kernregeln

- Frage nach, wenn neue Sudoku-Strategien oder Ausgabeformate nicht klar definiert sind.
- Behalte die App klein und lokal verständlich.
- Jede neue Strategie braucht eine nachvollziehbare textliche Begründung und möglichst ein kleines Beispiel im Codekommentar oder in der Dokumentation.
- Neue Konfiguration gehört in `appsettings.json` und in passende Options-Klassen.
- Nach Codeänderungen mindestens `dotnet build`, `dotnet run` und `dotnet test` ausführen.
- Code ist mit möglichst vielen, aber nur sinnvollen und zweckdienlichen Kommentaren zu versehen.
- Kommentare sollen Logik, Annahmen, Strategie und nicht offensichtliche Entscheidungen erklären.
- Sinnlose, redundante oder bloß den Code wiederholende Kommentare sollen entfallen.

## Wie man an diesem Projekt arbeitet

- Build/Test: `dotnet restore`, `dotnet build`, `dotnet test`.
- Run: `dotnet run --project src/VibeConsoleApp`.
- Konfiguration: `src/VibeConsoleApp/appsettings.json`, `SUDOKU_`-Umgebungsvariablen, `--puzzle=<81 Zeichen>` über die Kommandozeile.
- Key-Dateien:
  - `src/VibeConsoleApp/Program.cs` – Host, DI, Logging, Konfiguration.
  - `src/VibeConsoleApp/Services/SudokuConsoleApp.cs` – Programmlogik, Eingabeauflösung, Ausgabe und Fehlerbehandlung.
  - `src/VibeConsoleApp/Services/SudokuAnalyzer.cs` – Strategie-Reihenfolge und Suggestion-Logik.
  - `src/VibeConsoleApp/Options/SudokuOptions.cs` – Optionen für Puzzle, PrettyPrintBoard, ExplainCandidates.
  - `src/VibeConsoleApp/Services/SudokuParser.cs` – Puzzle-Parsing und Validierung.
  - `src/VibeConsoleApp/Models/SudokuMoveSuggestion.cs` – Ergebnisstruktur für Vorschläge.
  - `tests/VibeConsoleApp.Tests/SudokuAnalyzerTests.cs` – Regressionstests und erwartetes Verhalten.
- Wenn du eine neue Strategie oder Ausgabeänderung einführst, frage vorher nach, wenn das Format oder die Domänenabsicht nicht klar ist.
- Änderungen sollen lokal und zielgerichtet bleiben: eine konkrete Verbesserung pro Commit, mit passenden Tests.
- Priorisiere Erklärbarkeit und nachvollziehbare Begründungen gegenüber maximaler Lösungsstärke.
- Stimme neue Solver-Erweiterungen auf das bestehende Ziel ab: einen Vorschlag für den nächsten plausiblen Zug.

## Guardrails

- Keine erfundenen Sudoku-Regeln oder Behauptungen.
- Keine versteckte Magie in der Begründung; der Text muss aus der Logik ableitbar sein.
- Keine Backtracking-Suche als Standardweg.
- Keine unnötigen NuGet-Pakete.
- Fehler bei ungültigem Puzzle klar melden.
