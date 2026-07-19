# Skill: csharp-console

## Zweck

Nutze diesen Skill für Implementierung in `SudokuMoveAdvisor`.

## Fachliche Leitlinie

Die Anwendung soll nicht wahllos lösen, sondern einen **menschlich nachvollziehbaren nächsten Zug** vorschlagen.

## Bevorzugte Strategien

Direkte Strategien zuerst:

1. Naked Single
2. Hidden Single in Zeile
3. Hidden Single in Spalte
4. Hidden Single in Box

Danach Reduktionsstrategien mit Folgezug:

5. Pointing Pair / Triple
6. Box/Line Reduction
7. Naked Pair
8. Hidden Pair

## Implementierungsregeln

- Parse das Sudoku robust aus 81 Zeichen.
- Erlaube `0` oder `.` für leere Felder.
- Trenne Parsing, Analyse und Konsolenausgabe.
- Gib Positionen für Menschen 1-basiert aus, intern darf 0-basiert gearbeitet werden.
- Jede Strategie liefert `Strategy`, `Reason`, `Row`, `Column` und `Value`.
- Reduktionsstrategien sollen die Kandidaten-Streichungen als Zwischenschritte ausgeben.

## Guardrails

- Keine Backtracking-Suche als Standardweg für den nächsten Zug.
- Keine Ausgabe ohne Begründung.
- Keine Fachlogik direkt in `Program.cs`.
- Bei ungültigem Puzzle mit klarer Fehlermeldung abbrechen.
