using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VibeConsoleApp.Options;

namespace VibeConsoleApp.Services;

/// <summary>
/// A console application service that analyzes a Sudoku puzzle and suggests the next move.
/// </summary>  
/// <remarks>
/// This service is designed to be run as a console application. It takes a Sudoku puzzle as input, either from command line arguments or from configuration, and analyzes it to suggest the next move.
/// It uses the SudokuParser to parse the input puzzle and the SudokuAnalyzer to determine the next move. The results are printed to the console, and detailed logging is provided for debugging and analysis purposes.
/// </remarks>  
/// <example>
/// Example usage:
/// <code>
/// var parser = new SudokuParser();
/// var analyzer = new SudokuAnalyzer();        
/// var options = Options.Create(new SudokuOptions { Puzzle = "53..7....6..195....98......6.8...6...34..8..6...3.4..1.7...2..6....28....419..5....8..79", PrettyPrintBoard = true, ExplainCandidates = true });
/// var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<SudokuConsoleApp>();
/// var app = new SudokuConsoleApp(parser, analyzer, options, logger);
/// var exitCode = await app.RunAsync(new string[0]);
/// </code>
/// </example>  
/// <seealso cref="SudokuParser"/>
/// <seealso cref="SudokuAnalyzer"/>    
/// <seealso cref="SudokuOptions"/>
/// <seealso cref="ILogger{TCategoryName}"/>
/// <seealso cref="IOptions{TOptions}"/>
public sealed class SudokuConsoleApp
{
    /// <summary>
    /// The SudokuParser service used to parse the input puzzle.
    /// </summary>
    private readonly SudokuParser _parser;

    /// <summary>
    /// The SudokuAnalyzer service used to analyze the parsed puzzle and suggest the next move.
    /// </summary>
    private readonly SudokuAnalyzer _analyzer;

    /// <summary>
    /// The SudokuOptions containing configuration settings for the application.
    /// </summary>  
    private readonly SudokuOptions _options;

    /// <summary>       
    /// The ILogger used for logging information, warnings, and errors during the execution of the application.
    /// </summary>
    private readonly ILogger<SudokuConsoleApp> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SudokuConsoleApp"/> class with the specified dependencies.
    /// </summary>
    /// <param name="parser">The SudokuParser service used to parse the input puzzle.</param>
    /// <param name="analyzer">The SudokuAnalyzer service used to analyze the parsed puzzle and suggest the next move.</param>
    /// <param name="options">The IOptions containing configuration settings for the application.</param>
    /// <param name="logger">The ILogger used for logging information, warnings, and errors during the execution of the application.</param>
    /// <exception cref="ArgumentNullException">Thrown when any of the provided dependencies are null.</exception>
    /// <remarks>
    /// This constructor is used to inject the required services and configuration into the SudokuConsoleApp.
    /// It ensures that all dependencies are provided and initializes the corresponding private fields.
    /// </remarks>  
    public SudokuConsoleApp(
        SudokuParser parser,
        SudokuAnalyzer analyzer,
        IOptions<SudokuOptions> options,
        ILogger<SudokuConsoleApp> logger)
    {
        _parser = parser;
        _analyzer = analyzer;
        _options = options.Value;
        _logger = logger;
    }

    /// <summary>
    /// Runs the Sudoku console application, analyzing the provided puzzle and suggesting the next move.
    /// </summary>
    /// <param name="args">The command line arguments passed to the application.</param>
    /// <returns>A Task representing the asynchronous operation, with an integer exit code indicating the result of the execution. A return value of 0 indicates success, while non-zero values indicate various error conditions.</returns>
    /// <exception cref="ArgumentException">Thrown when the input puzzle is invalid or cannot be parsed.</exception>
    /// <remarks>
    /// This method is the main entry point for the Sudoku console application. It resolves the puzzle from command line arguments or configuration, parses it, and analyzes it to suggest the next move.
    /// It handles exceptions gracefully, logging errors and providing user-friendly messages in the console.
    /// </remarks>
    /// <example>
    /// Example usage:
    /// <code>
    /// var exitCode = await app.RunAsync(new string[] { "--puzzle=53..7....6..195....98......6.8...6...34..8..6...3.4..1.7...2..6....28....419..5....8..79" });
    /// </code>     
    /// </example>
    public Task<int> RunAsync(string[] args)
    {
        // Log the start of the Sudoku analysis process
        _logger.LogInformation("Starte Sudoku-Analyse...");

        try
        {
            // Resolve the Sudoku puzzle from command line arguments or configuration
            var puzzle = ResolvePuzzle(args);
            // If no puzzle is provided, log an error and return an exit code of 1
            if (string.IsNullOrWhiteSpace(puzzle))
            {
                _logger.LogError("Bitte gib ein Sudoku an: per appsettings.json oder --Sudoku:Puzzle=<81 Zeichen>.");
                return Task.FromResult(1);
            }

            // Parse the provided puzzle string into a SudokuBoard object
            var board = _parser.Parse(puzzle);
            // If the PrettyPrintBoard option is enabled, print the current Sudoku board to the console
            if (_options.PrettyPrintBoard)
            {
                _logger.LogInformation("Aktuelles Sudoku:");
                _logger.LogInformation(board.ToPrettyString());
            }

            // Analyze the Sudoku board and suggest the next move
            var suggestion = _analyzer.SuggestNextMove(board);
            // If no suggestion is available, log an error and return an exit code of 2
            if (suggestion is null)
            {
                _logger.LogError("Kein nächster erklärbarer Zug gefunden. Unterstützt werden Singles, Pointing Pair/Triple, Box/Line Reduction, Naked Pair und Hidden Pair.");
                return Task.FromResult(2);
            }

            _logger.LogInformation("Vorgeschlagener Zug: Zeile {Row}, Spalte {Column} = {Value}", suggestion.Row, suggestion.Column, suggestion.Value);
            _logger.LogInformation("Strategie: {Strategy}", suggestion.Strategy);
            _logger.LogInformation("Begründung: {Reason}", suggestion.Reason);

            // If the ExplainCandidates option is enabled and there are candidates for the suggested move, print them to the console
            if (_options.ExplainCandidates && suggestion.Candidates.Count > 0)
            {
                _logger.LogInformation("Kandidaten im Zielfeld: {Candidates}", string.Join(", ", suggestion.Candidates));
            }

            // If there are elimination steps associated with the suggestion, log them to the console
            if (suggestion.EliminationSteps.Count > 0)
            {
                _logger.LogInformation("Zwischenschritte:");
                foreach (var step in suggestion.EliminationSteps)
                {
                    _logger.LogInformation("- {Step}", step);
                }
            }

            // Log the suggested move with detailed information for debugging and analysis
            _logger.LogInformation("Sudoku-Zug vorgeschlagen: R{Row} C{Column} = {Value} via {Strategy}", suggestion.Row, suggestion.Column, suggestion.Value, suggestion.Strategy);
            // Return an exit code of 0 to indicate successful execution
            return Task.FromResult(0);
        }
        // Catch any exceptions that occur during the execution of the application, log the error, and return an exit code of 1
        catch (Exception exception)
        {
            // Log the exception details for debugging and analysis
            _logger.LogError(exception, "Fehler bei der Sudoku-Analyse.");
            _logger.LogInformation("Fehler: {Message}", exception.Message);
            
            // Return an exit code of 1 to indicate an error occurred during execution
            return Task.FromResult(1);
        }
    }

    /// <summary>
    /// Resolves the Sudoku puzzle string from command line arguments or configuration.
    /// </summary>
    /// <param name="args">The command line arguments passed to the application.</param>
    /// <returns>The resolved Sudoku puzzle string, or null if no puzzle is provided.</returns>
    /// <remarks>
    /// This method checks the command line arguments for a "--puzzle" argument. If found
    /// it returns the associated value. If not found, it falls back to the puzzle string specified in the SudokuOptions configuration.
    /// </remarks>
    /// <example>
    /// Example usage:
    /// <code>
    /// var puzzle = ResolvePuzzle(new string[] { "--puzzle=53..7....6..195....98......6.8...6...34..8..6...3.4..1.7...2..6....28....419..5....8..79" });
    /// </code> 
    /// </example>
    private string? ResolvePuzzle(string[] args)
    {
        // Check for a direct command line argument specifying the puzzle
        var directArgument = args
            .Select(argument => argument.Split('=', 2))
            .FirstOrDefault(parts => parts.Length == 2 && string.Equals(parts[0], "--puzzle", StringComparison.OrdinalIgnoreCase));

        // If a direct argument is found, return its value; otherwise, return the puzzle from the configuration options
        if (directArgument is not null)
        {
            return directArgument[1];
        }

        // If no direct argument is found, return the puzzle from the configuration options
        return _options.Puzzle;
    }
}