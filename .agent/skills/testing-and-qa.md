# Skill: testing-and-qa

## Zweck

Nutze diesen Skill für Verifikation in `SudokuMoveAdvisor`.

## Prüfablauf

1. `dotnet restore`
2. `dotnet build`
3. `dotnet run --project src/VibeConsoleApp`
4. `dotnet test`

## Was geprüft werden muss

- Das Sudoku wird korrekt geparst.
- Ein bekannter nächster Zug wird korrekt erkannt.
- Ungültige Eingaben liefern verständliche Fehler.
- Die textliche Begründung passt zur Strategie.

## Review-Checkliste

- Ist der vorgeschlagene Zug fachlich korrekt?
- Ist die verwendete Strategie wirklich die zuerst passende Strategie?
- Ist die Erklärung nachvollziehbar und knapp?
- Sind Zeilen/Spalten für Benutzer 1-basiert ausgegeben?
- Wurde keine unnötige Komplexität eingebaut?
