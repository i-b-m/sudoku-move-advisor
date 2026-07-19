namespace VibeConsoleApp.Options;

/// <summary>
/// Represents the configuration options for the Sudoku application, including the puzzle string, whether to explain candidates, and whether to pretty print the board.
/// </summary>  
/// <remarks>
/// This class is used to encapsulate the configuration settings for the Sudoku application. It includes properties for the puzzle string, whether to explain candidates, and whether to pretty print the board. The options can be configured through appsettings.json, environment variables, or command line arguments.
/// </remarks>
/// <example>
/// Example usage:
/// <code>
/// var options = new SudokuOptions
/// {
///     Puzzle = "53..7....6..195....98......6.8......6...34..8..6...3.4..1.7...2..6....28....419..5....8..79",
///     ExplainCandidates = true,
///     PrettyPrintBoard = true
/// };
/// </code>
/// </example>
public sealed class SudokuOptions
{
    /// <summary>
    /// The name of the configuration section for Sudoku options in the appsettings.json file.
    /// </summary>
    /// <value>The name of the configuration section for Sudoku options.</value>
    /// <example>
    /// Example usage:
    /// <code>
    /// var sectionName = SudokuOptions.SectionName; // Get the section name for Sudoku options
    /// </code>
    /// </example>  
    public const string SectionName = "Sudoku";

    /// <summary>
    /// Gets or sets the string representation of the Sudoku puzzle to be analyzed.
    /// </summary>  
    /// <value>The string representation of the Sudoku puzzle.</value>
    /// <example>
    /// Example usage:
    /// <code>
    /// var options = new SudokuOptions();
    /// options.Puzzle = "53..7....6..195....98......6.8...6...34..8..6...3.4..1.7...2...6....28....419..5....8..79"; // Set the puzzle string
    /// </code>
    /// </example>     
    public string? Puzzle { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to explain the candidates for each cell in the Sudoku puzzle.
    /// </summary>
    /// <value>A boolean value indicating whether to explain the candidates for each cell.</value>
    /// <example>
    /// Example usage:
    /// <code>
    /// var options = new SudokuOptions();
    /// options.ExplainCandidates = true; // Set to true to explain candidates
    /// </code>
    /// </example>
    public bool ExplainCandidates { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to pretty print the Sudoku board.
    /// </summary>
    /// <value>A boolean value indicating whether to pretty print the Sudoku board.</value>
    /// <example>
    /// Example usage:
    /// <code>
    /// var options = new SudokuOptions();
    /// options.PrettyPrintBoard = true; // Set to true to pretty print the board
    /// </code>
    /// </example>
    public bool PrettyPrintBoard { get; set; } = true;
}
