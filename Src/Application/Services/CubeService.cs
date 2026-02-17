namespace RubiksCube.Application.Services;

/// <summary>
/// Main service for Rubik's Cube operations.
/// Orchestrates domain operations and provides a clean API for the presentation layer.
/// </summary>
public class CubeService(Cube cube) : ICubeService
{
    private readonly Cube _cube = cube;

    /// <summary>
    /// Gets the cube instance
    /// </summary>
    public Cube Cube => _cube;

    /// <summary>
    /// Applies a single move to the cube
    /// </summary>
    /// <param name="move">The move to apply</param>
    public void ApplyMove(Move move)
    {
        _cube.ApplyMove(move);
    }

    /// <summary>
    /// Executes a sequence of moves on the cube
    /// </summary>
    /// <param name="sequence">The sequence of moves to execute</param>
    public void ExecuteSequence(MoveSequence sequence)
    {
        foreach (var move in sequence)
        {
            _cube.ApplyMove(move);
        }
    }
}
