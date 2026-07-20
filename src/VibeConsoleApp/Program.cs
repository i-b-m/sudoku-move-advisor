using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SudokuMoveAdvisor.Core.Services;
using VibeConsoleApp.Options;
using VibeConsoleApp.Services;

// <summary>
// The main entry point for the VibeConsoleApp application. 
// </summary>
namespace VibeConsoleApp;

internal static class Program
{
    private static async Task<int> Main(string[] args)
    {
        // Hilfe frueh pruefen: -h oder --help beendet das Programm sofort,
        // ohne Konfiguration oder Logging zu initialisieren.
        if (args.Any(a => a is "-h" or "--help" or "/?" or "help"))
        {
            HelpText.Print();
            return 0; 
        }

        // Create a host builder and configure services, logging, and configuration
        // This allows for dependency injection and configuration management
        using IHost host = Host.CreateDefaultBuilder(args)
            // Configure the application configuration
            .ConfigureAppConfiguration((context, config) =>
            {
                // Set the base path for configuration files to the application's base directory
                config.SetBasePath(AppContext.BaseDirectory);

                // Add the appsettings.json file to the configuration, making it required and not reloadable on change
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

                // Add environment variables with the prefix "SUDOKU_" to the configuration
                config.AddEnvironmentVariables(prefix: "SUDOKU_");

                // Add command line arguments to the configuration if any are provided
                if (args.Length > 0)
                {
                    // Add command line arguments to the configuration
                    config.AddCommandLine(args);
                }
            })
            // Configure services for dependency injection
            .ConfigureServices((context, services) =>
            {
                // Configure Sudoku options from the configuration section
                services.Configure<SudokuOptions>(context.Configuration.GetSection(SudokuOptions.SectionName));

                // Register services for dependency injection
                services.AddSingleton<SudokuParser>();

                // Register the SudokuAnalyzer service as a singleton
                services.AddSingleton<SudokuAnalyzer>();

                // Register the SudokuConsoleApp service as a singleton
                services.AddSingleton<SudokuConsoleApp>();
            })
            // Configure logging for the application
            .ConfigureLogging((context, logging) =>
            {
                // Clear default logging providers and add console logging
                logging.ClearProviders();

                // Add console logging to the logging configuration
                logging.AddConsole();
            })
            // Build the host
            .Build();

        // Retrieve the SudokuConsoleApp service from the host's service provider and run the application
        var app = host.Services.GetRequiredService<SudokuConsoleApp>();

        // Run the application and return the exit code
        return await app.RunAsync(args);
    }
}
