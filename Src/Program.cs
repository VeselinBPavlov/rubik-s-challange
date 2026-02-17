using Microsoft.Extensions.DependencyInjection;
using RubiksCube.Application.Services;
using RubiksCube.Infrastructure.Rendering;
using RubiksCube.Presentation.Controllers;

namespace RubiksCube;

class Program
{
    static void Main()
    {
        try
        {
            // Configure dependency injection
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();

            // Run challenge
            var controller = serviceProvider.GetRequiredService<CubeController>();
            controller.RunChallenge();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nError: {ex.Message}");
            Console.ResetColor();
            Console.ReadKey();
        }
    }

    private static void ConfigureServices(ServiceCollection services)
    {
        // Domain
        services.AddSingleton(Cube.CreateSolved());

        // Application
        services.AddSingleton<ICubeService, CubeService>();
        services.AddSingleton<IOrchestrator, Orchestrator>();

        // Infrastructure
        services.AddSingleton<ICubeRenderer, ConsoleCubeRenderer>();

        // Presentation
        services.AddSingleton<CubeController>();
    }
}
