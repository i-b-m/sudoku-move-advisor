# SudokuMoveAdvisor

`SudokuMoveAdvisor` ist eine einfache C#-Konsolenanwendung für VS Code, die für ein gegebenes Sudoku den **nächsten sinnvollen Spielzug** vorschlägt und kurz begründet.

## Projektidee

Die App analysiert ein Sudoku und sucht nach nachvollziehbaren Strategien. Dabei soll nicht nur ein Wert vorgeschlagen werden, sondern auch erklärt werden, **warum** dieser Zug jetzt sinnvoll ist.

## Unterstützte Strategien

Direkte Strategien:

- Naked Single
- Hidden Single in Zeile
- Hidden Single in Spalte
- Hidden Single in 3x3-Box

Reduktionsstrategien mit Folgezug:

- Pointing Pair / Pointing Triple
- Box/Line Reduction
- Naked Pair
- Hidden Pair
- X-Wing
- Swordfish

Bei den Reduktionsstrategien erklärt die App zuerst die Kandidaten-Streichungen und nennt dann den daraus folgenden konkreten Zug.

## Strategiebeispiele

### Naked Single

Beispielidee: In einem Feld sind nach Prüfung von Zeile, Spalte und 3x3-Box alle Zahlen bis auf eine ausgeschlossen. Dann ist dieses Feld sofort lösbar.

```text
Naked Single für Kandidat 5

        C1  C2  C3 | C4  C5  C6 | C7  C8  C9
      -------------+------------+-------------
R1      8   3   1  |  6   7   2 |  9   4   .
R2      6   7   2  |  1  [5]  9 |  3   8   4
R3      9   4   5  |  3   8   4 |  1   2   7
      -------------+------------+-------------
R4      1   2   3  |  7   9   5 |  4   6   8
R5      4   5   6  |  8   1   3 |  2   7   9
R6      7   8   9  |  2   4   6 |  5   1   3
      -------------+------------+-------------
R7      2   1   4  |  5   3   7 |  8   9   6
R8      3   6   7  |  9   2   8 |  0   5   1
R9      5   9   8  |  4   6   1 |  7   3   2
```

Lesart:
- Im markierten Feld bleibt nach Ausschluss nur noch `5` übrig.
- Es ist kein weiterer Vergleich mit anderen Kandidaten desselben Hauses nötig.
- Das ist der direkteste mögliche Zug.

### Hidden Single in Zeile

Beispielidee: Eine Zahl ist in einer Zeile nur in einem einzigen Feld noch möglich, auch wenn dort mehrere Kandidaten stehen. Dann muss diese Zahl genau dort eingetragen werden.

```text
Hidden Single in Zeile für Kandidat 6

        C1  C2  C3 | C4  C5  C6 | C7  C8  C9
      -------------+------------+-------------
R1      .   .   .  |  .   .   . |  .   .   .
R2      .  {2,6} . | {1,4} .  {3,8} | {5,7} {1,2} {4,9}
R3      .   .   .  |  .   .   . |  .   .   .
      -------------+------------+-------------
R4      .   .   .  |  .   .   . |  .   .   .
R5      .   .   .  |  .   .   . |  .   .   .
R6      .   .   .  |  .   .   . |  .   .   .
      -------------+------------+-------------
R7      .   .   .  |  .   .   . |  .   .   .
R8      .   .   .  |  .   .   . |  .   .   .
R9      .   .   .  |  .   .   . |  .   .   .
```

Lesart:
- In Zeile 2 kommt die `6` nur im Feld R2C2 vor.
- Auch wenn dort noch andere Kandidaten stehen, ist `6` dort versteckt eindeutig.
- Deshalb ist R2C2 = `6`.

### Hidden Single in Spalte

Beispielidee: Eine Zahl ist in einer Spalte nur in einem einzigen Feld möglich. Dann ist dieses Feld festgelegt.

```text
Hidden Single in Spalte für Kandidat 4

        C1  C2  C3 | C4  C5  C6 | C7  C8  C9
      -------------+------------+-------------
R1      .   .   .  | {1,4} .   . |  .   .   .
R2      .   .   .  | {2,7} .   . |  .   .   .
R3      .   .   .  | {3,8} .   . |  .   .   .
      -------------+------------+-------------
R4      .   .   .  | {1,6} .   . |  .   .   .
R5      .   .   .  | {5,9} .   . |  .   .   .
R6      .   .   .  | [4]   .   . |  .   .   .
      -------------+------------+-------------
R7      .   .   .  | {2,3} .   . |  .   .   .
R8      .   .   .  | {1,7} .   . |  .   .   .
R9      .   .   .  | {6,8} .   . |  .   .   .
```

Lesart:
- In Spalte 4 kommt die `4` nur in R6C4 vor.
- Deshalb ist dieses Feld festgelegt, auch wenn andere Zellen in der Spalte ebenfalls offen sind.
- Das ist ein Hidden Single über die Spalte.

### Hidden Single in 3x3-Box

Beispielidee: In einer 3x3-Box ist eine Zahl nur in einem Feld möglich. Dann muss sie dort gesetzt werden.

```text
Hidden Single in 3x3-Box für Kandidat 9

        C1  C2  C3 | C4  C5  C6 | C7  C8  C9
      -------------+------------+-------------
R1     {1,2} {3,4} {5,6} |  .   .   . |  .   .   .
R2     {2,5} [9]  {1,7} |  .   .   . |  .   .   .
R3     {4,6} {1,8} {2,3} |  .   .   . |  .   .   .
      -------------+------------+-------------
R4      .   .   .  |  .   .   . |  .   .   .
R5      .   .   .  |  .   .   . |  .   .   .
R6      .   .   .  |  .   .   . |  .   .   .
      -------------+------------+-------------
R7      .   .   .  |  .   .   . |  .   .   .
R8      .   .   .  |  .   .   . |  .   .   .
R9      .   .   .  |  .   .   . |  .   .   .
```

Lesart:
- In der linken oberen 3x3-Box taucht Kandidat `9` nur im Feld R2C2 auf.
- Deshalb ist R2C2 = `9`.
- Der entscheidende Vergleich findet nur innerhalb der Box statt.

### Pointing Pair / Pointing Triple

Beispielidee: Innerhalb einer 3x3-Box liegt ein Kandidat nur in einer einzigen Zeile oder Spalte. Dann kann dieser Kandidat außerhalb der Box aus derselben Zeile oder Spalte gestrichen werden.

```text
Pointing Pair für Kandidat 8

        C1  C2  C3 | C4  C5  C6 | C7  C8  C9
      -------------+------------+-------------
R1      .   .   .  | [8] [8]  . |  .   .   .   <- Kandidat 8 liegt in der Box nur in R1
R2      .   .   .  |  .   .   . |  .   .   .
R3      .   .   .  |  .   .   . |  .   .   .
      -------------+------------+-------------
R4      8   .   .  |  .   .   . |  .   .   .   <- diese 8 in R4C1 bleibt unberührt
R5      .   .   .  |  8   .   8 |  .   .   .   <- 8 in dieser Zeile, aber außerhalb der Box, wird gestrichen
R6      .   .   .  |  .   .   . |  .   .   .
      -------------+------------+-------------
R7      .   .   .  |  .   .   . |  .   .   .
R8      .   .   .  |  .   .   . |  .   .   .
R9      .   .   .  |  .   .   . |  .   .   .
```

Lesart:
- In der oberen mittleren Box kann die `8` nur in Zeile 1 stehen.
- Daher wird `8` in Zeile 1 außerhalb dieser Box gestrichen.
- So entsteht oft anschließend ein Single.

### Box/Line Reduction

Beispielidee: In einer Zeile oder Spalte kommt ein Kandidat nur innerhalb einer einzigen 3x3-Box vor. Dann kann der Kandidat in der restlichen Box außerhalb dieser Zeile oder Spalte gestrichen werden.

```text
Box/Line Reduction für Kandidat 3

        C1  C2  C3 | C4  C5  C6 | C7  C8  C9
      -------------+------------+-------------
R1      .   .   .  | [3]  .  [3] |  .   .   .   <- in Zeile 1 liegt Kandidat 3 nur in dieser Box
R2      .   .   .  |  3   .   . |  .   .   .   <- diese 3 in derselben Box wird gestrichen
R3      .   .   .  |  .   3   . |  .   .   .   <- diese 3 in derselben Box wird gestrichen
      -------------+------------+-------------
R4      .   .   .  |  .   .   . |  .   .   .
R5      .   .   .  |  .   .   . |  .   .   .
R6      .   .   .  |  .   .   . |  .   .   .
      -------------+------------+-------------
R7      .   .   .  |  .   .   . |  .   .   .
R8      .   .   .  |  .   .   . |  .   .   .
R9      .   .   .  |  .   .   . |  .   .   .
```

Lesart:
- In Zeile 1 kommt die `3` nur innerhalb der oberen mittleren Box vor.
- Deshalb kann `3` in derselben Box aus den anderen Zeilen gestrichen werden.
- Das ist die Umkehrung des Pointing-Musters.

### Naked Pair

Beispielidee: Zwei Felder einer Einheit enthalten exakt dieselben zwei Kandidaten. Dann müssen diese beiden Zahlen dort untergebracht werden und können aus allen anderen Feldern der Einheit entfernt werden.

```text
Naked Pair mit Kandidaten 2/7

        C1   C2   C3 | C4   C5   C6 | C7   C8   C9
      --------------+--------------+--------------
R1     {1,5} [2,7] {3,6} | {2,4,7} {7,8} {1,9} | {2,3} {4,6} {5,8}
R2      .    [2,7]  .  |   .    .    . |   .    .    .
R3      .     .     .  |   .    .    . |   .    .    .
      --------------+--------------+--------------
R4      .     .     .  |   .    .    . |   .    .    .
R5      .     .     .  |   .    .    . |   .    .    .
R6      .     .     .  |   .    .    . |   .    .    .
      --------------+--------------+--------------
R7      .     .     .  |   .    .    . |   .    .    .
R8      .     .     .  |   .    .    . |   .    .    .
R9      .     .     .  |   .    .    . |   .    .    .
```

Lesart:
- Die markierten Felder enthalten beide genau `{2,7}`.
- Damit sind `2` und `7` in dieser Einheit bereits belegt.
- Alle anderen Vorkommen von `2` oder `7` in derselben Einheit können gestrichen werden.

### Hidden Pair

Beispielidee: Zwei Zahlen kommen in einer Einheit nur in denselben zwei Feldern vor. Dann dürfen in diesen beiden Feldern alle anderen Kandidaten entfernt werden.

```text
Hidden Pair für Kandidaten 4/9

        C1    C2    C3 | C4    C5    C6 | C7    C8    C9
      ---------------+---------------+---------------
R1     {1,2} {3,5} {6,7} | {1,8} {2,6} {3,7} | {5,8} {1,2} {6,7}
R2     {2,3} [4,6,9] {1,5} | {2,7} [1,4,8,9] {3,6} | {2,5} {7,8} {1,6}
R3     {1,5} {2,6} {3,7} | {5,8} {1,2} {6,7} | {2,3} {5,6} {7,8}
      ---------------+---------------+---------------
R4      .     .     .  |   .     .     . |   .     .     .
R5      .     .     .  |   .     .     . |   .     .     .
R6      .     .     .  |   .     .     . |   .     .     .
      ---------------+---------------+---------------
R7      .     .     .  |   .     .     . |   .     .     .
R8      .     .     .  |   .     .     . |   .     .     .
R9      .     .     .  |   .     .     . |   .     .     .
```

Lesart:
- In der betrachteten Einheit kommen `4` und `9` nur in den beiden markierten Feldern vor.
- Deshalb bilden diese beiden Zahlen dort ein Hidden Pair.
- Andere Kandidaten in diesen beiden Feldern dürfen gestrichen werden.

### X-Wing

Beispielidee: Kandidat `7` kommt in **Zeile 2** nur in **Spalte 3 und 8** vor und in **Zeile 5** ebenfalls nur in **Spalte 3 und 8**. Dann bilden diese vier Positionen ein Rechteck. Die `7` muss innerhalb dieses Rechtecks in einer der beiden Zeilen liegen; deshalb darf `7` in **allen anderen Feldern von Spalte 3 und 8** gestrichen werden.

```text
X-Wing für Kandidat 7

        C1  C2  C3 | C4  C5  C6 | C7  C8  C9
      -------------+------------+-------------
R1      .   .   7  |  .   .   . |  .   7   .   <- diese 7er werden gestrichen
R2      .   .  [7] |  .   .   . |  .  [7]  .   <- Teil des X-Wing
R3      .   .   .  |  .   .   . |  .   .   .
      -------------+------------+-------------
R4      .   .   .  |  .   .   . |  .   .   .
R5      .   .  [7] |  .   .   . |  .  [7]  .   <- Teil des X-Wing
R6      .   .   7  |  .   .   . |  .   7   .   <- diese 7er werden gestrichen
      -------------+------------+-------------
R7      .   .   .  |  .   .   . |  .   .   .
R8      .   .   7  |  .   .   . |  .   .   .   <- diese 7 wird gestrichen
R9      .   .   .  |  .   .   . |  .   .   .
```

Lesart:
- Die markierten `[7]` in R2/R5 und C3/C8 bilden das Rechteck.
- Daher müssen die echten 7er in diesen beiden Spalten auf den beiden X-Wing-Zeilen liegen.
- Alle anderen `7` in Spalte 3 und 8 können entfernt werden.

### Swordfish

Beispielidee: Kandidat `4` kommt in **Zeile 1, 4 und 7** nur innerhalb der **Spalten 2, 5 und 9** vor. Dann bilden diese drei Zeilen und drei Spalten ein Swordfish-Muster. Deshalb darf `4` aus **allen anderen Feldern in Spalte 2, 5 und 9** gestrichen werden.

```text
Swordfish für Kandidat 4

        C1  C2  C3 | C4  C5  C6 | C7  C8  C9
      -------------+------------+-------------
R1      .  [4]  .  |  .  [4]  . |  .   .  [4]  <- Teil des Swordfish
R2      .   4   .  |  .   .   . |  .   .   .   <- diese 4 wird gestrichen
R3      .   .   .  |  .   .   . |  .   .   .
      -------------+------------+-------------
R4      .  [4]  .  |  .  [4]  . |  .   .  [4]  <- Teil des Swordfish
R5      .   .   .  |  .   4   . |  .   .   .   <- diese 4 wird gestrichen
R6      .   .   .  |  .   .   . |  .   .   .
      -------------+------------+-------------
R7      .  [4]  .  |  .  [4]  . |  .   .  [4]  <- Teil des Swordfish
R8      .   .   .  |  .   .   . |  .   .   4   <- diese 4 wird gestrichen
R9      .   .   .  |  .   .   . |  .   .   .
```

Lesart:
- In den Zeilen 1, 4 und 7 kommt Kandidat `4` nur in den Spalten 2, 5 und 9 vor.
- Diese drei Zeilen koppeln sich damit an genau diese drei Spalten.
- Alle anderen `4` in Spalte 2, 5 und 9 können gestrichen werden.

## Technologie

- .NET 8 als aktueller stabiler Stack
- `appsettings.json` für Konfiguration
- `Microsoft.Extensions.Logging` für Konsolen-Logging
- xUnit als Testframework
- VS Code Tasks und Launch-Konfiguration

## Start

```bash
dotnet restore
dotnet build
dotnet run --project src/VibeConsoleApp
dotnet test
```

## Puzzle übergeben

Standardmäßig liest die App das Sudoku aus `appsettings.json`.

Alternativ per CLI:

```bash
dotnet run --project src/VibeConsoleApp -- --puzzle=530070000600195000098000060800060003400803001700020006060000280000419005000080079
```

## Ausgabe

Die App zeigt:

- das aktuelle Sudoku
- den vorgeschlagenen Zug
- die verwendete Strategie
- eine textliche Begründung
- Kandidaten des Zielfelds
- bei Bedarf Zwischenschritte zur Kandidatenreduktion

## Hinweis zu xUnit

xUnit ist ein verbreitetes Testframework für .NET. Zum Ausführen der vorhandenen Tests reicht `dotnet test`.
