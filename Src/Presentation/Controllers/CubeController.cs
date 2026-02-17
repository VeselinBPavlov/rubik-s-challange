namespace RubiksCube.Presentation.Controllers;

/// <summary>
/// Controller for managing the Rubik's Cube challenge workflow and user interaction.
/// </summary>
public class CubeController(
    ICubeService cubeService,
    IOrchestrator orchestrator,
    ICubeRenderer renderer)
{
    /// <summary>
    /// Runs the complete challenge workflow with interactive output
    /// </summary>
    public void RunChallenge()
    {
        ShowWelcome();
        renderer.Render(cubeService.Cube, "Initial State (Solved Cube)");
        WaitForUser();

        ShowSequence();
        WaitForUser();

        ExecuteSteps();

        ShowCompletion();
    }

    private static void ShowWelcome()
    {
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════╗");
        Console.WriteLine("║   Rubik's Cube Simulator - Challenge   ║");
        Console.WriteLine("╚════════════════════════════════════════╝");
        Console.WriteLine();
        Console.WriteLine("Green (Front) • Red (Right) • White (Up)");
        Console.WriteLine();
    }

    private void ShowSequence()
    {
        var sequence = orchestrator.GetChallengeSequence();
        Console.WriteLine();
        Console.WriteLine($"Sequence: {sequence}");
        Console.WriteLine();
        Console.WriteLine(sequence.GetDetailedDescription());
        Console.WriteLine();
    }

    private void ExecuteSteps()
    {
        orchestrator.ExecuteChallengeStepByStep((move, step) =>
        {
            Console.Clear();
            Console.WriteLine($"Step {step}/6: {move.GetDescription()}");
            Console.WriteLine(new string('─', 50));

            renderer.Render(cubeService.Cube);

            if (step < 6) WaitForUser();
        });
    }

    private void ShowCompletion()
    {
        Console.WriteLine();
        Console.WriteLine("╔════════════════════════════════╗");
        Console.WriteLine("║      Challenge Completed!      ║");
        Console.WriteLine("╚════════════════════════════════╝");
        renderer.Render(cubeService.Cube, "Final State");
    }

    private static void WaitForUser()
    {
        Console.WriteLine();
        Console.Write("Press any key to continue...");
        Console.ResetColor();
        Console.ReadKey(true);
        Console.WriteLine();
    }
}
