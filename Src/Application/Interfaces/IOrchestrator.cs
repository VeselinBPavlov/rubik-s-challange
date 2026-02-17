namespace RubiksCube.Application.Interfaces;

/// <summary>
/// Orchestrates cube operation workflows.
/// Separates workflow-specific logic from general cube operations.
/// </summary>
public interface IOrchestrator
{
    /// <summary>
    /// Executes the complete challenge sequence
    /// </summary>
    void ExecuteChallenge();

    /// <summary>
    /// Executes the challenge sequence step by step, invoking a callback after each move
    /// </summary>
    /// <param name="onMoveExecuted">Callback invoked after each move with the move and step number</param>
    void ExecuteChallengeStepByStep(Action<Move, int> onMoveExecuted);

    /// <summary>
    /// Gets the challenge move sequence
    /// </summary>
    MoveSequence GetChallengeSequence();
}
