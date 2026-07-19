using VibeConsoleApp.Models;

namespace VibeConsoleApp.Services;

/// <summary>
/// A service that analyzes a Sudoku puzzle and suggests the next move based on various solving strategies.
/// </summary>
/// <remarks>
/// This service is designed to be used in conjunction with the SudokuParser and SudokuConsoleApp services. It takes a SudokuBoard object as input and applies a series of solving strategies to determine the next best move. The strategies include Naked Singles, Hidden Singles, Pointing Pairs/Triples, Box/Line Reductions, Naked Pairs, Hidden Pairs, X-Wing, and Swordfish.
/// </remarks>
/// <example>
/// Example usage:
/// <code>
/// var parser = new SudokuParser();
/// var analyzer = new SudokuAnalyzer();
/// var sudokuBoard = parser.Parse("53..7....6..195....98......6.8...6...34..8..6...3.4..1.7...2..6....28....419..5....8..79");
/// var suggestion = analyzer.SuggestNextMove(sudokuBoard);
/// </code>
/// </example>
public sealed class SudokuAnalyzer
{
    /// <summary>
    /// Analyzes the given Sudoku board and suggests the next move based on various solving strategies.
    /// </summary>
    /// <param name="board">The SudokuBoard object representing the current state of the Sudoku puzzle.</param>
    /// <returns>A SudokuMoveSuggestion object representing the suggested next move, or null if no move can be suggested.</returns>
    /// <remarks>
    /// This method applies a series of solving strategies to the provided Sudoku board. It first builds a candidate map for each empty cell, then checks for Naked Singles, Hidden Singles in rows, columns, and boxes, Pointing Pairs/Triples, Box/Line Reductions, Naked Pairs, Hidden Pairs, X-Wing patterns, and Swordfish patterns. The first valid move found is returned as a suggestion.
    /// </remarks>
    /// <example>
    /// Example usage:
    /// <code>
    /// var parser = new SudokuParser();
    /// var analyzer = new SudokuAnalyzer();
    /// var sudokuBoard = parser.Parse("53..7....6..195....98......6.8...6...34..8..6...3.4..1.7...2..6....28....419..5....8..79");
    /// var suggestion = analyzer.SuggestNextMove(sudokuBoard);
    /// </code>
    /// </example>
    public SudokuMoveSuggestion? SuggestNextMove(SudokuBoard board)
    {
        // Build a candidate map for each empty cell in the Sudoku board
        var candidateMap = BuildCandidateMap(board);

        // Apply various solving strategies in order of complexity and return the first valid move found
        return FindNakedSingle(candidateMap)
            ?? FindHiddenSingleInRows(candidateMap)
            ?? FindHiddenSingleInColumns(candidateMap)
            ?? FindHiddenSingleInBoxes(candidateMap)
            ?? FindPointingPairOrTriple(candidateMap)
            ?? FindBoxLineReduction(candidateMap)
            ?? FindNakedPairDrivenMove(candidateMap)
            ?? FindHiddenPairDrivenMove(candidateMap)
            ?? FindXWing(candidateMap)
            ?? FindSwordfish(candidateMap);
    }

    /// <summary>
    /// Builds a candidate map for each empty cell in the given Sudoku board.
    /// </summary>
    /// <param name="board">The SudokuBoard object representing the current state of the Sudoku puzzle.</param>
    /// <returns>A dictionary mapping each empty cell's coordinates (row, column) to a list of possible candidate values.</returns>
    /// <remarks>
    /// This method iterates through each cell in the Sudoku board. For each empty cell, it determines the possible candidate values by excluding values already present in the same row, column, and 3x3 box. The resulting candidate map is used by other methods to suggest the next move.
    /// </remarks>
    /// <example>
    /// Example usage:
    /// <code>
    /// var parser = new SudokuParser();
    /// var analyzer = new SudokuAnalyzer();
    /// var sudokuBoard = parser.Parse("53..7....6..195....98......6.8...6...34..8..6...3.4..1.7...2..6....28....419..5....8..79");
    /// var candidateMap = analyzer.BuildCandidateMap(sudokuBoard);
    /// </code>
    /// </example>
    private Dictionary<(int Row, int Column), List<int>> BuildCandidateMap(SudokuBoard board)
    {
        // Initialize a dictionary to hold the candidate values for each empty cell
        var result = new Dictionary<(int Row, int Column), List<int>>();

        // Iterate through each cell in the 9x9 Sudoku board
        for (var row = 0; row < 9; row++)
        {
            // For each column in the current row, check if the cell is empty
            for (var column = 0; column < 9; column++)
            {
                // If the cell is not empty, skip to the next cell
                if (!board.IsEmpty(row, column))
                {
                    continue;
                }

                // Determine the values already used in the same row, column, and 3x3 box
                var used = board.GetRowValues(row)
                    .Concat(board.GetColumnValues(column))
                    .Concat(board.GetBoxValues(row, column))
                    .ToHashSet();

                // Determine the possible candidate values for the empty cell by excluding used values
                var candidates = Enumerable.Range(1, 9)
                    .Where(value => !used.Contains(value))
                    .ToList();

                // Add the empty cell's coordinates and its candidate values to the result dictionary
                result[(row, column)] = candidates;
            }
        }

        return result;
    }

    /// <summary>
    /// Finds a Naked Single move in the given candidate map.
    /// </summary>
    /// <param name="candidateMap">A dictionary mapping each empty cell's coordinates (row, column) to a list of possible candidate values.</param>
    /// <returns>A SudokuMoveSuggestion object representing the Naked Single move, or null if no Naked Single is found.</returns>
    /// <remarks>
    /// A Naked Single occurs when an empty cell has only one possible candidate value. This method searches the candidate map for such cells and returns the first Naked Single found as a suggested move.
    /// </remarks>
    /// <example>
    /// Example usage:
    /// <code>
    /// var parser = new SudokuParser();
    /// var analyzer = new SudokuAnalyzer();
    /// var sudokuBoard = parser.Parse("53..7....6..195....98......6.8...6...34..8..6...3.4..1.7...2..6....28....419..5....8..79");
    /// var candidateMap = analyzer.BuildCandidateMap(sudokuBoard);
    /// var nakedSingleSuggestion = analyzer.FindNakedSingle(candidateMap);
    /// </code>
    /// </example>
    private static SudokuMoveSuggestion? FindNakedSingle(Dictionary<(int Row, int Column), List<int>> candidateMap)
    {
        // Search for the first entry in the candidate map where the list of candidates has exactly one value
        var match = candidateMap.FirstOrDefault(entry => entry.Value.Count == 1);
        // If no such entry is found, return null
        if (match.Equals(default(KeyValuePair<(int Row, int Column), List<int>>)))
        {
            return null;
        }

        // Extract the single candidate value for the Naked Single cell
        var value = match.Value[0];
        
        // Return a SudokuMoveSuggestion object representing the Naked Single move
        return new SudokuMoveSuggestion
        {
            Row = match.Key.Row + 1,
            Column = match.Key.Column + 1,
            Value = value,
            Strategy = "Naked Single",
            Reason = $"In Zeile {match.Key.Row + 1}, Spalte {match.Key.Column + 1} bleibt nach Ausschluss über Zeile, Spalte und 3x3-Box nur die Zahl {value} übrig.",
            Candidates = match.Value
        };
    }

    /// <summary>
    /// Finds a Hidden Single move in the rows of the given candidate map.
    /// </summary>
    /// <param name="candidateMap">A dictionary mapping each empty cell's coordinates (row, column) to a list of possible candidate values.</param>
    /// <returns>A SudokuMoveSuggestion object representing the Hidden Single move in a row, or null if no Hidden Single is found.</returns>
    /// <remarks>
    /// A Hidden Single occurs when a candidate value can only appear in one cell within a row, even if that cell has multiple candidates. This method searches the candidate map for such cases in each row and returns the first Hidden Single found as a suggested move.
    /// </remarks>
    /// <example>
    /// Example usage:
    /// <code>
    /// var parser = new SudokuParser();
    /// var analyzer = new SudokuAnalyzer();
    /// var sudokuBoard = parser.Parse("53..7....6..195....98......6.8...6...34..8..6...3.4..1.7...2..6....28....419..5....8..79");
    /// var candidateMap = analyzer.BuildCandidateMap(sudokuBoard);
    /// var hiddenSingleSuggestion = analyzer.FindHiddenSingleInRows(candidateMap);
    /// </code>
    /// </example>
    private static SudokuMoveSuggestion? FindHiddenSingleInRows(Dictionary<(int Row, int Column), List<int>> candidateMap)
    {
        // Iterate through each row of the Sudoku board
        for (var row = 0; row < 9; row++)
        {
            // Get all entries in the candidate map that belong to the current row
            var rowEntries = candidateMap.Where(entry => entry.Key.Row == row).ToList();
            for (var value = 1; value <= 9; value++)
            {
                // Find all entries in the current row that contain the candidate value
                var matches = rowEntries.Where(entry => entry.Value.Contains(value)).ToList();
                if (matches.Count == 1)
                {
                    // If exactly one match is found, it indicates a Hidden Single in the row. Create and return a SudokuMoveSuggestion for this move.
                    var cell = matches[0];

                    // Return a SudokuMoveSuggestion object representing the Hidden Single move in the row
                    return new SudokuMoveSuggestion
                    {
                        Row = cell.Key.Row + 1,
                        Column = cell.Key.Column + 1,
                        Value = value,
                        Strategy = "Hidden Single (Row)",
                        Reason = $"In Zeile {row + 1} kann die Zahl {value} nur in Spalte {cell.Key.Column + 1} stehen.",
                        Candidates = cell.Value
                    };
                }
            }
        }

        return null;
    }

    private static SudokuMoveSuggestion? FindHiddenSingleInColumns(Dictionary<(int Row, int Column), List<int>> candidateMap)
    {
        for (var column = 0; column < 9; column++)
        {
            var columnEntries = candidateMap.Where(entry => entry.Key.Column == column).ToList();
            for (var value = 1; value <= 9; value++)
            {
                var matches = columnEntries.Where(entry => entry.Value.Contains(value)).ToList();
                if (matches.Count == 1)
                {
                    var cell = matches[0];
                    return new SudokuMoveSuggestion
                    {
                        Row = cell.Key.Row + 1,
                        Column = cell.Key.Column + 1,
                        Value = value,
                        Strategy = "Hidden Single (Column)",
                        Reason = $"In Spalte {column + 1} kann die Zahl {value} nur in Zeile {cell.Key.Row + 1} stehen.",
                        Candidates = cell.Value
                    };
                }
            }
        }

        return null;
    }

    private static SudokuMoveSuggestion? FindHiddenSingleInBoxes(Dictionary<(int Row, int Column), List<int>> candidateMap)
    {
        for (var boxRow = 0; boxRow < 3; boxRow++)
        {
            for (var boxColumn = 0; boxColumn < 3; boxColumn++)
            {
                var startRow = boxRow * 3;
                var startColumn = boxColumn * 3;
                var boxEntries = candidateMap
                    .Where(entry => entry.Key.Row >= startRow && entry.Key.Row < startRow + 3 && entry.Key.Column >= startColumn && entry.Key.Column < startColumn + 3)
                    .ToList();

                for (var value = 1; value <= 9; value++)
                {
                    var matches = boxEntries.Where(entry => entry.Value.Contains(value)).ToList();
                    if (matches.Count == 1)
                    {
                        var cell = matches[0];
                        return new SudokuMoveSuggestion
                        {
                            Row = cell.Key.Row + 1,
                            Column = cell.Key.Column + 1,
                            Value = value,
                            Strategy = "Hidden Single (Box)",
                            Reason = $"In der 3x3-Box ({boxRow + 1}, {boxColumn + 1}) kann die Zahl {value} nur in Zeile {cell.Key.Row + 1}, Spalte {cell.Key.Column + 1} stehen.",
                            Candidates = cell.Value
                        };
                    }
                }
            }
        }

        return null;
    }

    private static SudokuMoveSuggestion? FindPointingPairOrTriple(Dictionary<(int Row, int Column), List<int>> candidateMap)
    {
        for (var boxRow = 0; boxRow < 3; boxRow++)
        {
            for (var boxColumn = 0; boxColumn < 3; boxColumn++)
            {
                var startRow = boxRow * 3;
                var startColumn = boxColumn * 3;
                var boxEntries = candidateMap
                    .Where(entry => entry.Key.Row >= startRow && entry.Key.Row < startRow + 3 && entry.Key.Column >= startColumn && entry.Key.Column < startColumn + 3)
                    .ToList();

                for (var value = 1; value <= 9; value++)
                {
                    var matches = boxEntries.Where(entry => entry.Value.Contains(value)).ToList();
                    if (matches.Count < 2 || matches.Count > 3)
                    {
                        continue;
                    }

                    var distinctRows = matches.Select(match => match.Key.Row).Distinct().ToList();
                    if (distinctRows.Count == 1)
                    {
                        var targetRow = distinctRows[0];
                        var outsideBoxEntries = candidateMap
                            .Where(entry => entry.Key.Row == targetRow && (entry.Key.Column < startColumn || entry.Key.Column >= startColumn + 3))
                            .Where(entry => entry.Value.Contains(value))
                            .ToList();

                        if (outsideBoxEntries.Count > 0)
                        {
                            var reducedMap = CloneCandidateMap(candidateMap);
                            foreach (var entry in outsideBoxEntries)
                            {
                                reducedMap[entry.Key].Remove(value);
                            }

                            var followUp = FindFollowUpSingle(reducedMap);
                            if (followUp is not null)
                            {
                                return CreateDerivedSuggestion(
                                    followUp,
                                    "Pointing Pair/Triple",
                                    $"In der 3x3-Box ({boxRow + 1}, {boxColumn + 1}) liegt Kandidat {value} nur in Zeile {targetRow + 1}. Daher kann {value} außerhalb der Box aus dieser Zeile gestrichen werden. Dadurch ergibt sich der Zug.",
                                    outsideBoxEntries.Select(entry => $"Entferne {value} aus Zeile {targetRow + 1}, Spalte {entry.Key.Column + 1}.").ToList());
                            }
                        }
                    }

                    var distinctColumns = matches.Select(match => match.Key.Column).Distinct().ToList();
                    if (distinctColumns.Count == 1)
                    {
                        var targetColumn = distinctColumns[0];
                        var outsideBoxEntries = candidateMap
                            .Where(entry => entry.Key.Column == targetColumn && (entry.Key.Row < startRow || entry.Key.Row >= startRow + 3))
                            .Where(entry => entry.Value.Contains(value))
                            .ToList();

                        if (outsideBoxEntries.Count > 0)
                        {
                            var reducedMap = CloneCandidateMap(candidateMap);
                            foreach (var entry in outsideBoxEntries)
                            {
                                reducedMap[entry.Key].Remove(value);
                            }

                            var followUp = FindFollowUpSingle(reducedMap);
                            if (followUp is not null)
                            {
                                return CreateDerivedSuggestion(
                                    followUp,
                                    "Pointing Pair/Triple",
                                    $"In der 3x3-Box ({boxRow + 1}, {boxColumn + 1}) liegt Kandidat {value} nur in Spalte {targetColumn + 1}. Daher kann {value} außerhalb der Box aus dieser Spalte gestrichen werden. Dadurch ergibt sich der Zug.",
                                    outsideBoxEntries.Select(entry => $"Entferne {value} aus Zeile {entry.Key.Row + 1}, Spalte {targetColumn + 1}.").ToList());
                            }
                        }
                    }
                }
            }
        }

        return null;
    }

    private static SudokuMoveSuggestion? FindBoxLineReduction(Dictionary<(int Row, int Column), List<int>> candidateMap)
    {
        for (var row = 0; row < 9; row++)
        {
            var rowEntries = candidateMap.Where(entry => entry.Key.Row == row).ToList();
            for (var value = 1; value <= 9; value++)
            {
                var matches = rowEntries.Where(entry => entry.Value.Contains(value)).ToList();
                if (matches.Count < 2 || matches.Count > 3)
                {
                    continue;
                }

                var distinctBoxes = matches.Select(entry => (entry.Key.Row / 3, entry.Key.Column / 3)).Distinct().ToList();
                if (distinctBoxes.Count == 1)
                {
                    var (boxRow, boxColumn) = distinctBoxes[0];
                    var startRow = boxRow * 3;
                    var startColumn = boxColumn * 3;
                    var outsideRowInsideBox = candidateMap
                        .Where(entry => entry.Key.Row >= startRow && entry.Key.Row < startRow + 3 && entry.Key.Column >= startColumn && entry.Key.Column < startColumn + 3)
                        .Where(entry => entry.Key.Row != row && entry.Value.Contains(value))
                        .ToList();

                    if (outsideRowInsideBox.Count > 0)
                    {
                        var reducedMap = CloneCandidateMap(candidateMap);
                        foreach (var entry in outsideRowInsideBox)
                        {
                            reducedMap[entry.Key].Remove(value);
                        }

                        var followUp = FindFollowUpSingle(reducedMap);
                        if (followUp is not null)
                        {
                            return CreateDerivedSuggestion(
                                followUp,
                                "Box/Line Reduction",
                                $"In Zeile {row + 1} kommt Kandidat {value} nur innerhalb der 3x3-Box ({boxRow + 1}, {boxColumn + 1}) vor. Daher kann {value} in der übrigen Box außerhalb dieser Zeile gestrichen werden. Dadurch ergibt sich der Zug.",
                                outsideRowInsideBox.Select(entry => $"Entferne {value} aus Zeile {entry.Key.Row + 1}, Spalte {entry.Key.Column + 1}.").ToList());
                        }
                    }
                }
            }
        }

        for (var column = 0; column < 9; column++)
        {
            var columnEntries = candidateMap.Where(entry => entry.Key.Column == column).ToList();
            for (var value = 1; value <= 9; value++)
            {
                var matches = columnEntries.Where(entry => entry.Value.Contains(value)).ToList();
                if (matches.Count < 2 || matches.Count > 3)
                {
                    continue;
                }

                var distinctBoxes = matches.Select(entry => (entry.Key.Row / 3, entry.Key.Column / 3)).Distinct().ToList();
                if (distinctBoxes.Count == 1)
                {
                    var (boxRow, boxColumn) = distinctBoxes[0];
                    var startRow = boxRow * 3;
                    var startColumn = boxColumn * 3;
                    var outsideColumnInsideBox = candidateMap
                        .Where(entry => entry.Key.Row >= startRow && entry.Key.Row < startRow + 3 && entry.Key.Column >= startColumn && entry.Key.Column < startColumn + 3)
                        .Where(entry => entry.Key.Column != column && entry.Value.Contains(value))
                        .ToList();

                    if (outsideColumnInsideBox.Count > 0)
                    {
                        var reducedMap = CloneCandidateMap(candidateMap);
                        foreach (var entry in outsideColumnInsideBox)
                        {
                            reducedMap[entry.Key].Remove(value);
                        }

                        var followUp = FindFollowUpSingle(reducedMap);
                        if (followUp is not null)
                        {
                            return CreateDerivedSuggestion(
                                followUp,
                                "Box/Line Reduction",
                                $"In Spalte {column + 1} kommt Kandidat {value} nur innerhalb der 3x3-Box ({boxRow + 1}, {boxColumn + 1}) vor. Daher kann {value} in der übrigen Box außerhalb dieser Spalte gestrichen werden. Dadurch ergibt sich der Zug.",
                                outsideColumnInsideBox.Select(entry => $"Entferne {value} aus Zeile {entry.Key.Row + 1}, Spalte {entry.Key.Column + 1}.").ToList());
                        }
                    }
                }
            }
        }

        return null;
    }

    private static SudokuMoveSuggestion? FindNakedPairDrivenMove(Dictionary<(int Row, int Column), List<int>> candidateMap)
    {
        foreach (var unit in EnumerateUnits(candidateMap))
        {
            var pairs = unit
                .Where(entry => entry.Value.Count == 2)
                .GroupBy(entry => string.Join(',', entry.Value.OrderBy(value => value)))
                .FirstOrDefault(group => group.Count() == 2);

            if (pairs is null)
            {
                continue;
            }

            var pairValues = pairs.First().Value.OrderBy(value => value).ToList();
            var pairKeys = pairs.Select(entry => entry.Key).ToHashSet();
            var otherEntries = unit.Where(entry => !pairKeys.Contains(entry.Key)).ToList();
            var affectedEntries = otherEntries.Where(entry => pairValues.Any(entry.Value.Contains)).ToList();

            if (affectedEntries.Count == 0)
            {
                continue;
            }

            var reducedMap = CloneCandidateMap(candidateMap);
            foreach (var entry in affectedEntries)
            {
                reducedMap[entry.Key] = reducedMap[entry.Key].Except(pairValues).ToList();
            }

            var followUp = FindFollowUpSingle(reducedMap);
            if (followUp is not null)
            {
                return CreateDerivedSuggestion(
                    followUp,
                    "Naked Pair",
                    $"In {DescribeUnit(unit)} bilden die Kandidaten {pairValues[0]} und {pairValues[1]} ein Naked Pair. Diese beiden Zahlen können daher aus den anderen Feldern der Einheit gestrichen werden. Dadurch ergibt sich der Zug.",
                    affectedEntries.Select(entry => $"Entferne {string.Join("/", pairValues)} aus Zeile {entry.Key.Row + 1}, Spalte {entry.Key.Column + 1}.").ToList());
            }
        }

        return null;
    }

    private static SudokuMoveSuggestion? FindHiddenPairDrivenMove(Dictionary<(int Row, int Column), List<int>> candidateMap)
    {
        foreach (var unit in EnumerateUnits(candidateMap))
        {
            for (var first = 1; first <= 8; first++)
            {
                for (var second = first + 1; second <= 9; second++)
                {
                    var firstMatches = unit.Where(entry => entry.Value.Contains(first)).ToList();
                    var secondMatches = unit.Where(entry => entry.Value.Contains(second)).ToList();
                    if (firstMatches.Count != 2 || secondMatches.Count != 2)
                    {
                        continue;
                    }

                    var firstKeys = firstMatches.Select(entry => entry.Key).OrderBy(key => key.Row).ThenBy(key => key.Column).ToList();
                    var secondKeys = secondMatches.Select(entry => entry.Key).OrderBy(key => key.Row).ThenBy(key => key.Column).ToList();
                    if (!firstKeys.SequenceEqual(secondKeys))
                    {
                        continue;
                    }

                    if (firstMatches.All(cell => cell.Value.Count == 2))
                    {
                        continue;
                    }

                    var reducedMap = CloneCandidateMap(candidateMap);
                    foreach (var cell in firstMatches)
                    {
                        reducedMap[cell.Key] = reducedMap[cell.Key].Where(value => value == first || value == second).ToList();
                    }

                    var followUp = FindFollowUpSingle(reducedMap);
                    if (followUp is not null)
                    {
                        return CreateDerivedSuggestion(
                            followUp,
                            "Hidden Pair",
                            $"In {DescribeUnit(unit)} können die Zahlen {first} und {second} nur in zwei gemeinsamen Feldern stehen. Daher bleiben dort nur diese beiden Kandidaten erhalten. Dadurch ergibt sich der Zug.",
                            firstMatches.Select(entry => $"Reduziere Zeile {entry.Key.Row + 1}, Spalte {entry.Key.Column + 1} auf {first}/{second}.").ToList());
                    }
                }
            }
        }

        return null;
    }

    // X-Wing Beispiel (ASCII):
    //
    //         C1  C2  C3 | C4  C5  C6 | C7  C8  C9
    //       -------------+------------+-------------
    // R2      .   .  [7] |  .   .   . |  .  [7]  .
    // R5      .   .  [7] |  .   .   . |  .  [7]  .
    //
    // Weitere 7er in C3 oder C8 außerhalb von R2/R5 dürfen gestrichen werden,
    // weil die vier markierten Kandidaten ein X-Wing-Rechteck bilden.
    private static SudokuMoveSuggestion? FindXWing(Dictionary<(int Row, int Column), List<int>> candidateMap)
    {
        for (var value = 1; value <= 9; value++)
        {
            var rowPatterns = Enumerable.Range(0, 9)
                .Select(row => new
                {
                    Row = row,
                    Columns = candidateMap.Where(entry => entry.Key.Row == row && entry.Value.Contains(value))
                        .Select(entry => entry.Key.Column)
                        .Distinct()
                        .OrderBy(column => column)
                        .ToList()
                })
                .Where(item => item.Columns.Count == 2)
                .ToList();

            for (var first = 0; first < rowPatterns.Count - 1; first++)
            {
                for (var second = first + 1; second < rowPatterns.Count; second++)
                {
                    if (!rowPatterns[first].Columns.SequenceEqual(rowPatterns[second].Columns))
                    {
                        continue;
                    }

                    var targetColumns = rowPatterns[first].Columns;
                    var affectedEntries = candidateMap
                        .Where(entry => targetColumns.Contains(entry.Key.Column)
                                        && entry.Value.Contains(value)
                                        && entry.Key.Row != rowPatterns[first].Row
                                        && entry.Key.Row != rowPatterns[second].Row)
                        .ToList();

                    if (affectedEntries.Count == 0)
                    {
                        continue;
                    }

                    var reducedMap = CloneCandidateMap(candidateMap);
                    foreach (var entry in affectedEntries)
                    {
                        reducedMap[entry.Key].Remove(value);
                    }

                    var followUp = FindFollowUpSingle(reducedMap);
                    if (followUp is not null)
                    {
                        return CreateDerivedSuggestion(
                            followUp,
                            "X-Wing",
                            $"Kandidat {value} bildet ein X-Wing über Zeile {rowPatterns[first].Row + 1} und Zeile {rowPatterns[second].Row + 1} sowie Spalte {targetColumns[0] + 1} und Spalte {targetColumns[1] + 1}. Deshalb kann {value} aus allen anderen Feldern dieser beiden Spalten gestrichen werden. Dadurch ergibt sich der Zug.",
                            affectedEntries.Select(entry => $"Entferne {value} aus Zeile {entry.Key.Row + 1}, Spalte {entry.Key.Column + 1}.").ToList());
                    }
                }
            }

            var columnPatterns = Enumerable.Range(0, 9)
                .Select(column => new
                {
                    Column = column,
                    Rows = candidateMap.Where(entry => entry.Key.Column == column && entry.Value.Contains(value))
                        .Select(entry => entry.Key.Row)
                        .Distinct()
                        .OrderBy(row => row)
                        .ToList()
                })
                .Where(item => item.Rows.Count == 2)
                .ToList();

            for (var first = 0; first < columnPatterns.Count - 1; first++)
            {
                for (var second = first + 1; second < columnPatterns.Count; second++)
                {
                    if (!columnPatterns[first].Rows.SequenceEqual(columnPatterns[second].Rows))
                    {
                        continue;
                    }

                    var targetRows = columnPatterns[first].Rows;
                    var affectedEntries = candidateMap
                        .Where(entry => targetRows.Contains(entry.Key.Row)
                                        && entry.Value.Contains(value)
                                        && entry.Key.Column != columnPatterns[first].Column
                                        && entry.Key.Column != columnPatterns[second].Column)
                        .ToList();

                    if (affectedEntries.Count == 0)
                    {
                        continue;
                    }

                    var reducedMap = CloneCandidateMap(candidateMap);
                    foreach (var entry in affectedEntries)
                    {
                        reducedMap[entry.Key].Remove(value);
                    }

                    var followUp = FindFollowUpSingle(reducedMap);
                    if (followUp is not null)
                    {
                        return CreateDerivedSuggestion(
                            followUp,
                            "X-Wing",
                            $"Kandidat {value} bildet ein X-Wing über Spalte {columnPatterns[first].Column + 1} und Spalte {columnPatterns[second].Column + 1} sowie Zeile {targetRows[0] + 1} und Zeile {targetRows[1] + 1}. Deshalb kann {value} aus allen anderen Feldern dieser beiden Zeilen gestrichen werden. Dadurch ergibt sich der Zug.",
                            affectedEntries.Select(entry => $"Entferne {value} aus Zeile {entry.Key.Row + 1}, Spalte {entry.Key.Column + 1}.").ToList());
                    }
                }
            }
        }

        return null;
    }

    // Swordfish Beispiel (ASCII):
    //
    //         C1  C2  C3 | C4  C5  C6 | C7  C8  C9
    //       -------------+------------+-------------
    // R1      .  [4]  .  |  .  [4]  . |  .   .  [4]
    // R4      .  [4]  .  |  .  [4]  . |  .   .  [4]
    // R7      .  [4]  .  |  .  [4]  . |  .   .  [4]
    //
    // Weitere 4er in C2, C5 oder C9 außerhalb von R1/R4/R7 dürfen gestrichen werden,
    // weil diese drei Zeilen zusammen ein Swordfish-Muster bilden.
    private static SudokuMoveSuggestion? FindSwordfish(Dictionary<(int Row, int Column), List<int>> candidateMap)
    {
        for (var value = 1; value <= 9; value++)
        {
            var rowPatterns = Enumerable.Range(0, 9)
                .Select(row => new
                {
                    Row = row,
                    Columns = candidateMap.Where(entry => entry.Key.Row == row && entry.Value.Contains(value))
                        .Select(entry => entry.Key.Column)
                        .Distinct()
                        .OrderBy(column => column)
                        .ToList()
                })
                .Where(item => item.Columns.Count is >= 2 and <= 3)
                .ToList();

            var rowResult = FindSwordfishCore(
                value,
                rowPatterns.Select(item => (Index: item.Row, Related: item.Columns)).ToList(),
                candidateMap,
                byRows: true);
            if (rowResult is not null)
            {
                return rowResult;
            }

            var columnPatterns = Enumerable.Range(0, 9)
                .Select(column => new
                {
                    Column = column,
                    Rows = candidateMap.Where(entry => entry.Key.Column == column && entry.Value.Contains(value))
                        .Select(entry => entry.Key.Row)
                        .Distinct()
                        .OrderBy(row => row)
                        .ToList()
                })
                .Where(item => item.Rows.Count is >= 2 and <= 3)
                .ToList();

            var columnResult = FindSwordfishCore(
                value,
                columnPatterns.Select(item => (Index: item.Column, Related: item.Rows)).ToList(),
                candidateMap,
                byRows: false);
            if (columnResult is not null)
            {
                return columnResult;
            }
        }

        return null;
    }

    private static SudokuMoveSuggestion? FindSwordfishCore(
        int value,
        List<(int Index, List<int> Related)> patterns,
        Dictionary<(int Row, int Column), List<int>> candidateMap,
        bool byRows)
    {
        for (var first = 0; first < patterns.Count - 2; first++)
        {
            for (var second = first + 1; second < patterns.Count - 1; second++)
            {
                for (var third = second + 1; third < patterns.Count; third++)
                {
                    var selected = new[] { patterns[first], patterns[second], patterns[third] };
                    var combined = selected.SelectMany(item => item.Related).Distinct().OrderBy(item => item).ToList();
                    if (combined.Count != 3)
                    {
                        continue;
                    }

                    if (selected.Any(item => item.Related.Except(combined).Any()))
                    {
                        continue;
                    }

                    List<KeyValuePair<(int Row, int Column), List<int>>> affectedEntries;
                    if (byRows)
                    {
                        var targetRows = selected.Select(item => item.Index).ToHashSet();
                        var targetColumns = combined.ToHashSet();
                        affectedEntries = candidateMap
                            .Where(entry => targetColumns.Contains(entry.Key.Column)
                                            && entry.Value.Contains(value)
                                            && !targetRows.Contains(entry.Key.Row))
                            .ToList();
                    }
                    else
                    {
                        var targetColumns = selected.Select(item => item.Index).ToHashSet();
                        var targetRows = combined.ToHashSet();
                        affectedEntries = candidateMap
                            .Where(entry => targetRows.Contains(entry.Key.Row)
                                            && entry.Value.Contains(value)
                                            && !targetColumns.Contains(entry.Key.Column))
                            .ToList();
                    }

                    if (affectedEntries.Count == 0)
                    {
                        continue;
                    }

                    var reducedMap = CloneCandidateMap(candidateMap);
                    foreach (var entry in affectedEntries)
                    {
                        reducedMap[entry.Key].Remove(value);
                    }

                    var followUp = FindFollowUpSingle(reducedMap);
                    if (followUp is not null)
                    {
                        var indexes = selected.Select(item => item.Index + 1).OrderBy(item => item).ToList();
                        var related = combined.Select(item => item + 1).OrderBy(item => item).ToList();
                        var reason = byRows
                            ? $"Kandidat {value} bildet ein Swordfish über die Zeilen {string.Join(", ", indexes)} und die Spalten {string.Join(", ", related)}. Deshalb kann {value} aus allen anderen Feldern dieser Spalten gestrichen werden. Dadurch ergibt sich der Zug."
                            : $"Kandidat {value} bildet ein Swordfish über die Spalten {string.Join(", ", indexes)} und die Zeilen {string.Join(", ", related)}. Deshalb kann {value} aus allen anderen Feldern dieser Zeilen gestrichen werden. Dadurch ergibt sich der Zug.";

                        return CreateDerivedSuggestion(
                            followUp,
                            "Swordfish",
                            reason,
                            affectedEntries.Select(entry => $"Entferne {value} aus Zeile {entry.Key.Row + 1}, Spalte {entry.Key.Column + 1}.").ToList());
                    }
                }
            }
        }

        return null;
    }

    private static SudokuMoveSuggestion CreateDerivedSuggestion(
        SudokuMoveSuggestion followUp,
        string strategy,
        string reason,
        IReadOnlyList<string> eliminationSteps)
    {
        return new SudokuMoveSuggestion
        {
            Row = followUp.Row,
            Column = followUp.Column,
            Value = followUp.Value,
            Candidates = followUp.Candidates,
            Strategy = strategy,
            Reason = reason,
            EliminationSteps = eliminationSteps
        };
    }

    private static SudokuMoveSuggestion? FindFollowUpSingle(Dictionary<(int Row, int Column), List<int>> candidateMap)
        => FindNakedSingle(candidateMap)
           ?? FindHiddenSingleInRows(candidateMap)
           ?? FindHiddenSingleInColumns(candidateMap)
           ?? FindHiddenSingleInBoxes(candidateMap);

    private static Dictionary<(int Row, int Column), List<int>> CloneCandidateMap(Dictionary<(int Row, int Column), List<int>> candidateMap)
        => candidateMap.ToDictionary(entry => entry.Key, entry => entry.Value.ToList());

    private static IEnumerable<List<KeyValuePair<(int Row, int Column), List<int>>>> EnumerateUnits(Dictionary<(int Row, int Column), List<int>> candidateMap)
    {
        for (var row = 0; row < 9; row++)
        {
            yield return candidateMap.Where(entry => entry.Key.Row == row).ToList();
        }

        for (var column = 0; column < 9; column++)
        {
            yield return candidateMap.Where(entry => entry.Key.Column == column).ToList();
        }

        for (var boxRow = 0; boxRow < 3; boxRow++)
        {
            for (var boxColumn = 0; boxColumn < 3; boxColumn++)
            {
                var startRow = boxRow * 3;
                var startColumn = boxColumn * 3;
                yield return candidateMap
                    .Where(entry => entry.Key.Row >= startRow && entry.Key.Row < startRow + 3 && entry.Key.Column >= startColumn && entry.Key.Column < startColumn + 3)
                    .ToList();
            }
        }
    }

    private static string DescribeUnit(List<KeyValuePair<(int Row, int Column), List<int>>> unit)
    {
        if (unit.Select(entry => entry.Key.Row).Distinct().Count() == 1)
        {
            return $"Zeile {unit[0].Key.Row + 1}";
        }

        if (unit.Select(entry => entry.Key.Column).Distinct().Count() == 1)
        {
            return $"Spalte {unit[0].Key.Column + 1}";
        }

        var first = unit[0].Key;
        return $"3x3-Box ({first.Row / 3 + 1}, {first.Column / 3 + 1})";
    }
}
