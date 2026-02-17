namespace RubiksCube.Application.Interfaces;

/// <summary>
/// Service for managing Rubik's Cube operations.
/// Orchestrates domain logic and provides a clean API for the presentation layer.
/// </summary>
public interface ICubeService
{
    /// <summary>
    /// Gets the cube instance
    /// </summary>
    Cube Cube { get; }

    /// <summary>
    /// Applies a single move to the cube
    /// </summary>
    /// <param name="move">The move to apply</param>
    void ApplyMove(Move move);

    /// <summary>
    /// Executes a sequence of moves on the cube
    /// </summary>
    /// <param name="sequence">The sequence of moves to execute</param>
    void ExecuteSequence(MoveSequence sequence);
}
