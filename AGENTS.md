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

## Guardrails

- Keine erfundenen Sudoku-Regeln oder Behauptungen.
- Keine versteckte Magie in der Begründung; der Text muss aus der Logik ableitbar sein.
- Keine Backtracking-Suche als Standardweg.
- Keine unnötigen NuGet-Pakete.
- Fehler bei ungültigem Puzzle klar melden.
