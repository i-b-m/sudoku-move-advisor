using SudokuMoveAdvisor.Core.Models;

namespace SudokuMoveAdvisor.Core.Services;

/// <summary>
/// A service that parses a string representation of a Sudoku puzzle into a SudokuBoard object.
/// </summary>
public sealed class SudokuParser
{
    /// <summary>
    /// Parses a string representation of a Sudoku puzzle into a SudokuBoard object.
    /// </summary>
    /// <param name="input">The string representation of the Sudoku puzzle.</param>
    /// <returns>A SudokuBoard object representing the parsed Sudoku puzzle.</returns>
    /// <exception cref="ArgumentException">Thrown when the input string does not contain exactly 81 characters (digits or '.' for empty cells).</exception>        
    /// <remarks>
    /// The input string should contain exactly 81 characters, where each character is either a digit (1-9) representing a filled cell or '.' (or '0') representing an empty cell. The method will throw an ArgumentException if the input string does not meet these criteria.
    /// </remarks>
    /// <example>
    /// Example usage:
    /// <code>
    /// var parser = new SudokuParser();
    /// var sudokuBoard = parser.Parse("53..7....6..195....98......6.8...6...34..8..6...3.4..1.7...2..6....28....419..5....8..79");
    /// </code>
    public SudokuBoard Parse(string input)
    {
        // Filter the input string to include only digits and '.' characters, and convert it to a character array
        var digits = input.Where(character => char.IsDigit(character) || character == '.').ToArray();
        if (digits.Length != 81)
        {
            throw new ArgumentException("Das Sudoku muss genau 81 Zeichen enthalten. Erlaubt sind Ziffern, 0 oder . für leere Felder.");
        }

        // Create a 9x9 array to hold the Sudoku cells
        var cells = new int[9, 9];
        for (var index = 0; index < digits.Length; index++)
        {
            var row = index / 9;
            var column = index % 9;
            cells[row, column] = digits[index] is '.' or '0' ? 0 : digits[index] - '0';
        }

        // Return a new SudokuBoard object initialized with the parsed cells
        return new SudokuBoard(cells);
    }
}
