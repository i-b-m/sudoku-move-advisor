namespace SudokuMoveAdvisor.Core.Models;

/// <summary>
/// Represents a suggested move in a Sudoku puzzle, including the row, column, value, strategy, reason, candidates, and elimination steps.
/// </summary>  
/// <remarks>
/// This class is used to encapsulate the details of a suggested move in a Sudoku puzzle,
/// including the row and column of the cell, the value to be placed, the strategy used to determine the move, the reason for the suggestion, any candidates for the cell, and any elimination steps that led to the suggestion.
/// </remarks>
/// <example>
/// Example usage:
/// <code>
/// var suggestion = new SudokuMoveSuggestion
/// {
///    Row = 1,
///    Column = 2,
///    Value = 5,
///    Strategy = "Hidden Single",
///    Reason = "Only candidate for this cell",
///    Candidates = new List<int> { 5 },
///    EliminationSteps = new List<string> { "Eliminated 1, 2, 3, 4, 6, 7, 8, 9" }
/// };
/// </code>
/// </example>
public sealed class SudokuMoveSuggestion
{
    /// <summary>
    /// Gets or sets the row index (1-based) of the suggested move.
    /// </summary>
    /// <value>The row index (1-based) of the suggested move.</value>
    /// <example>
    /// Example usage:
    /// <code>
    /// var suggestion = new SudokuMoveSuggestion();
    /// suggestion.Row = 1; // Set the row index to 1 (first row)
    /// </code>
    /// </example>
    public required int Row { get; init; }

    /// <summary>
    /// Gets or sets the column index (1-based) of the suggested move.
    /// </summary>
    /// <value>The column index (1-based) of the suggested move.</value>
    /// <example>
    /// Example usage:
    /// <code>
    /// var suggestion = new SudokuMoveSuggestion();
    /// suggestion.Column = 2; // Set the column index to 2 (second column)
    /// </code>
    /// </example>
    public required int Column { get; init; }

    /// <summary>
    /// Gets or sets the value to be placed in the suggested move.
    /// </summary>
    /// <value>The value to be placed in the suggested move.</value>
    /// <example>
    /// Example usage:
    /// <code>
    /// var suggestion = new SudokuMoveSuggestion();
    /// suggestion.Value = 5; // Set the value to be placed in the cell to 5
    /// </code>
    /// </example>
    public required int Value { get; init; }

    /// <summary>
    /// Gets or sets the strategy used to determine the suggested move.
    /// </summary>  
    /// <value>The strategy used to determine the suggested move.</value>
    /// <example>
    /// Example usage:
    /// <code>
    /// var suggestion = new SudokuMoveSuggestion();
    /// suggestion.Strategy = "Hidden Single"; // Set the strategy used to determine the move
    /// </code>
    /// </example>
    public required string Strategy { get; init; }

    /// <summary>
    /// Gets or sets the reason for the suggested move.
    /// </summary>
    /// <value>The reason for the suggested move.</value>
    /// <example>
    /// Example usage:
    /// <code>
    /// var suggestion = new SudokuMoveSuggestion();
    /// suggestion.Reason = "Only candidate for this cell"; // Set the reason for the suggested move
    /// </code>
    /// </example>
    public required string Reason { get; init; }

    /// <summary>
    /// Gets or sets the list of candidates for the suggested move.
    /// </summary>
    /// <value>The list of candidates for the suggested move.</value>
    /// <example>
    /// Example usage:
    /// <code>
    /// var suggestion = new SudokuMoveSuggestion();
    /// suggestion.Candidates = new List<int> { 5 }; // Set the candidates for the suggested move
    /// </code>
    /// </example>
    public IReadOnlyList<int> Candidates { get; init; } = Array.Empty<int>();

    /// <summary>
    /// Gets or sets the list of elimination steps that led to the suggested move.
    /// </summary>
    /// <value>The list of elimination steps that led to the suggested move.</value>
    /// <example>
    /// Example usage:
    /// <code>
    /// var suggestion = new SudokuMoveSuggestion();
    /// suggestion.EliminationSteps = new List<string> { "Eliminated 1, 2, 3, 4, 6, 7, 8, 9" }; // Set the elimination steps that led to the suggested move
    /// </code>
    /// </example>
    public IReadOnlyList<string> EliminationSteps { get; init; } = Array.Empty<string>();
}
