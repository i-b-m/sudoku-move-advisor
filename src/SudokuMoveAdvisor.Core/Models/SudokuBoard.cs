namespace SudokuMoveAdvisor.Core.Models;

/// <summary>
/// Represents a Sudoku board with methods to access cell values and retrieve row, column, and box values.
/// </summary>  
/// <remarks>
/// The SudokuBoard class encapsulates the state of a Sudoku puzzle, providing methods to access individual
/// cell values, check if a cell is empty, and retrieve the values present in a specific row, column, or 3x3 box.
/// It also provides a method to generate a pretty string representation of the board for display purposes.
/// </remarks>  
/// <example>
/// Example usage:
/// <code>
/// var cells = new int[9, 9]
/// {
///     { 5, 3, 0, 0, 7, 0, 0, 0, 0 },
///     { 6, 0, 0, 1, 9, 5, 0, 0, 0 },
///     { 0, 9, 8, 0, 0, 0, 0, 6, 0 },
///     { 8, 0, 0, 0, 6, 0, 0, 0, 3 },
///     { 4, 0, 0, 8, 0, 3, 0, 0, 1 },
///     { 7, 0, 0, 0, 2, 0, 0, 0, 6 },
///     { 0, 6, 0, 0, 0, 0, 2, 8, 0 },
///     { 0, 0, 0, 4, 1, 9, 0, 0, 5 },
///     { 0,  0, 0, 0, 8, 0, 0, 7, 9 }
/// };
/// var board = new SudokuBoard(cells);
/// var prettyString = board.ToPrettyString();      
/// </code>
/// </example>

public sealed class SudokuBoard
{
    /// The internal 9x9 array representing the Sudoku board's cells, where 0 indicates an empty cell.
    private readonly int[,] _cells;

    /// Initializes a new instance of the <see cref="SudokuBoard"/> class with the specified 9x9 array of cells.
    public SudokuBoard(int[,] cells)
    {
        _cells = cells;
    }

    /// <summary>
    /// Gets the value of the cell at the specified row and column.
    /// </summary>
    /// <param name="row">The row index (0-based) of the cell.</param>
    /// <param name="column">The column index (0-based) of the cell.</param>
    /// <returns>The value of the cell, where 0 indicates an empty cell.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when the specified row or column index is out of bounds (not in the range 0-8).</exception>
    /// <remarks>
    /// This method retrieves the value of a specific cell in the Sudoku board. The row and column indices must be in the range of 0 to 8, inclusive. If the indices are out of bounds, an IndexOutOfRangeException will be thrown. The value returned is an integer representing the cell's content, where 0 indicates that the cell is empty.
    /// </remarks>
    /// <example>
    /// Example usage:
    /// <code>
    /// var board = new SudokuBoard(cells);
    /// int value = board.GetCell(0, 1); // Retrieves the value of the cell at row 0, column 1
    /// </code>
    /// </example>
    public int GetCell(int row, int column) => _cells[row, column];

    /// <summary>
    /// Determines whether the cell at the specified row and column is empty (i.e., has a value of 0).
    /// </summary>
    /// <param name="row">The row index (0-based) of the cell.</param>
    /// <param name="column">The column index (0-based) of the cell.</param>
    /// <returns>True if the cell is empty (value is 0); otherwise, false.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when the specified row or column index is out of bounds (not in the range 0-8).</exception>
    /// <remarks>
    /// This method checks whether a specific cell in the Sudoku board is empty. The row and column indices must be in the range of 0 to 8, inclusive. If the indices are out of bounds, an IndexOutOfRangeException will be thrown. The method returns true if the cell's value is 0 (indicating it is empty), and false otherwise.
    /// </remarks>
    /// <example>
    /// Example usage:
    /// <code>
    /// var board = new SudokuBoard(cells);
    /// bool isEmpty = board.IsEmpty(0, 1); // Checks if the cell at row 0, column 1 is empty
    /// </code>
    /// </example>
    public bool IsEmpty(int row, int column) => GetCell(row, column) == 0;

    /// <summary>
    /// Retrieves the values present in the specified row of the Sudoku board, excluding empty cells (value 0).
    /// </summary>
    /// <param name="row">The row index (0-based) for which to retrieve the values.</param>
    /// <returns>An enumerable collection of integers representing the values in the specified row, excluding empty cells.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when the specified row index is out of bounds (not in the range 0-8).</exception>
    /// <remarks>
    /// This method retrieves all the values present in a specific row of the Sudoku board, excluding any empty cells (cells with a value of 0). The row index must be in the range of 0 to 8, inclusive. If the index is out of bounds, an IndexOut
    /// OfRangeException will be thrown. The method returns an enumerable collection of integers representing the non-empty values in the specified row.
    /// </remarks>
    /// <example>
    /// Example usage:
    /// <code>
    /// var board = new SudokuBoard(cells);
    /// var rowValues = board.GetRowValues(0); // Retrieves the values in row
    /// </code>
    /// </example>  
    public IEnumerable<int> GetRowValues(int row)
    {
        for (var column = 0; column < 9; column++)
        {
            var value = _cells[row, column];
            if (value != 0)
            {
                yield return value;
            }
        }
    }

    /// <summary>
    /// Retrieves the values present in the specified column of the Sudoku board, excluding empty cells (value 0).
    /// </summary>
    /// <param name="column">The column index (0-based) for which to retrieve the values.</param>
    /// <returns>An enumerable collection of integers representing the values in the specified column, excluding empty cells.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when the specified column index is out of bounds (not in the range 0-8).</exception>
    /// <remarks>
    /// This method retrieves all the values present in a specific column of the Sudoku board, excluding any empty cells (cells with a value of 0). The column index must be in the range of 0 to 8, inclusive. If the index is out of bounds, an IndexOutOfRangeException will be thrown. The method returns an enumerable collection of integers representing the non-empty values in the specified column.
    /// </remarks>
    /// <example>
    /// Example usage:
    /// <code>
    /// var board = new SudokuBoard(cells);
    /// var columnValues = board.GetColumnValues(0); // Retrieves the values in column 0, excluding empty cells
    /// </code>
    /// </example>
    public IEnumerable<int> GetColumnValues(int column)
    {
        for (var row = 0; row < 9; row++)
        {
            var value = _cells[row, column];
            if (value != 0)
            {
                yield return value;
            }
        }
    }

    /// <summary>
    /// Retrieves the values present in the 3x3 box containing the specified cell, excluding empty cells (value 0).
    /// </summary>
    /// <param name="row">The row index (0-based) of the cell within the box.</param>
    /// <param name="column">The column index (0-based) of the cell within the box.</param>
    /// <returns>An enumerable collection of integers representing the values in the specified box, excluding empty cells.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when the specified row or column index is out of bounds (not in the range 0-8).</exception>
    /// <remarks>
    /// This method retrieves all the values present in a specific 3x3 box of the Sudoku board, excluding any empty cells (cells with a value of 0). The row and column indices must be in the range of 0 to 8, inclusive. If either index is out of bounds, an IndexOutOfRangeException will be thrown. The method returns an enumerable collection of integers representing the non-empty values in the specified box.
    /// </remarks>
    /// <example>
    /// Example usage:
    /// <code>
    /// var board = new SudokuBoard(cells);
    /// var boxValues = board.GetBoxValues(0, 0); // Retrieves the values in the top-left box, excluding empty cells
    /// </code>
    /// </example>
    public IEnumerable<int> GetBoxValues(int row, int column)
    {
        var startRow = row / 3 * 3;
        var startColumn = column / 3 * 3;

        for (var currentRow = startRow; currentRow < startRow + 3; currentRow++)
        {
            for (var currentColumn = startColumn; currentColumn < startColumn + 3; currentColumn++)
            {
                var value = _cells[currentRow, currentColumn];
                if (value != 0)
                {
                    yield return value;
                }
            }
        }
    }

    /// <summary>
    /// Generates a pretty string representation of the Sudoku board, with rows and columns formatted for easy reading.
    /// </summary>
    /// <returns>A string representing the Sudoku board in a human-readable format.</returns>
    /// <remarks>
    /// This method creates a formatted string representation of the Sudoku board, displaying the values in a grid format. Empty cells (value 0) are represented by a dot ('.'). The output includes horizontal and vertical separators to delineate the 3x3 boxes, making it easier to visualize the structure of the Sudoku puzzle. The resulting string can be printed to the console or used in logs for debugging purposes.
    /// </remarks>
    /// <example>
    /// Example usage:
    /// <code>
    /// var board = new SudokuBoard(cells);
    /// var prettyString = board.ToPrettyString(); // Generates a pretty string representation of the Sudoku board
    /// Console.WriteLine(prettyString); // Prints the formatted Sudoku board to the console
    /// </code>
    /// </example>      
    public string ToPrettyString()
    {
        // Create a list to hold the lines of the pretty string representation
        var lines = new List<string>();
        // Iterate through each row of the Sudoku board
        for (var row = 0; row < 9; row++)
        {
            // Add a horizontal separator line after every 3 rows, except for the first row
            if (row > 0 && row % 3 == 0)
            {
                lines.Add("------+-------+------");
            }

            // Create a list to hold the parts of the current row, including cell values and vertical separators
            var parts = new List<string>();
            for (var column = 0; column < 9; column++)
            {
                // Add a vertical separator after every 3 columns, except for the first column
                if (column > 0 && column % 3 == 0)
                {
                    parts.Add("|");
                }

                // Add the cell value to the parts list, using '.' for empty cells (value 0) and the actual value for filled cells
                parts.Add(_cells[row, column] == 0 ? "." : _cells[row, column].ToString());
            }

            // Join the parts of the current row into a single string and add it to the lines list
            lines.Add(string.Join(' ', parts));
        }

        // Join all the lines into a single string with newline separators and return the result
        return string.Join(Environment.NewLine, lines);
    }
}
