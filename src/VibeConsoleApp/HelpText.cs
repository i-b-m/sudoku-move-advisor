namespace VibeConsoleApp;

/// <summary>
/// Kapselt die Ausgabe der Kommandozeilen-Hilfe.
/// Getrennt vom Programmablauf gehalten, damit der Hilfetext an einer
/// einzigen, gut auffindbaren Stelle gepflegt werden kann.
/// </summary>
public static class HelpText
{
    // Aufrufname des Programms fuer die Beispielzeilen.
    // Bewusst konstant, damit Hilfe und Doku denselben Namen zeigen.
    private const string AppName = "SudokuMoveAdvisor";

    /// <summary>
    /// Gibt die vollstaendige Hilfe inkl. Optionen und Beispielen aus.
    /// </summary>
    public static void Print()
    {
        Console.WriteLine($@"{AppName} - schlaegt den naechsten sinnvollen Sudoku-Zug vor und begruendet ihn.

VERWENDUNG:
  dotnet run --project src/VibeConsoleApp -- [OPTIONEN]

  Als veroeffentlichte Binary:
  {AppName} [OPTIONEN]

OPTIONEN:
  -h, --help              Zeigt diese Hilfe an und beendet das Programm.
      --puzzle=<81 Zeichen> Uebergibt ein Sudoku direkt als 81-stelligen String.
                          Leere Felder werden als 0 dargestellt (9x9, zeilenweise).
                          Ueberschreibt den Wert aus der appsettings.json.

KONFIGURATION:
  Ohne --puzzle liest die App das Sudoku aus der appsettings.json:

    {{
      ""Sudoku"": {{
        ""Puzzle"": ""530070000600195000098000060800060003400803001700020006060000280000419005000080079"",
        ""ExplainCandidates"": true,
        ""PrettyPrintBoard"": true
      }}
    }}

BEISPIELE:
  Hilfe anzeigen:
    dotnet run --project src/VibeConsoleApp -- --help

  Sudoku aus appsettings.json analysieren:
    dotnet run --project src/VibeConsoleApp

  Sudoku direkt per Parameter uebergeben:
    dotnet run --project src/VibeConsoleApp -- --puzzle=530070000600195000098000060800060003400803001700020006060000280000419005000080079

  Format des Puzzle-Strings (9 Zeilen zu je 9 Zeichen, hier nur zur Veranschaulichung umgebrochen):
    530070000
    600195000
    098000060
    800060003
    400803001
    700020006
    060000280
    000419005
    000080079

HINWEIS:
  Die App schlaegt jeweils genau einen naechsten Zug vor und nennt die verwendete Strategie sowie eine kurze Begruendung.");
    }
}
