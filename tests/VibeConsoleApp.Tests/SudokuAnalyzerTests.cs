using SudokuMoveAdvisor.Core.Services;
using Xunit;

namespace VibeConsoleApp.Tests;

public sealed class SudokuAnalyzerTests
{
    [Fact]
    public void SuggestNextMove_ReturnsExpectedMove_ForSamplePuzzle()
    {
        var parser = new SudokuParser();
        var analyzer = new SudokuAnalyzer();
        var board = parser.Parse("530070000600195000098000060800060003400803001700020006060000280000419005000080079");

        var suggestion = analyzer.SuggestNextMove(board);

        Assert.NotNull(suggestion);
        Assert.Equal(5, suggestion!.Row);
        Assert.Equal(5, suggestion.Column);
        Assert.Equal(5, suggestion.Value);
        Assert.Equal("Naked Single", suggestion.Strategy);
    }

    [Fact]
    public void Parse_Throws_ForInvalidLength()
    {
        var parser = new SudokuParser();
        Assert.Throws<ArgumentException>(() => parser.Parse("123"));
    }

    [Fact]
    public void Suggestion_ContainsReason_WhenMoveFound()
    {
        var parser = new SudokuParser();
        var analyzer = new SudokuAnalyzer();
        var board = parser.Parse("530070000600195000098000060800060003400803001700020006060000280000419005000080079");

        var suggestion = analyzer.SuggestNextMove(board);

        Assert.NotNull(suggestion);
        Assert.False(string.IsNullOrWhiteSpace(suggestion!.Reason));
    }
}
