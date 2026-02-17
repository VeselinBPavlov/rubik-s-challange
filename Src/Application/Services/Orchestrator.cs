namespace RubiksCube.Application.Services;

/// <summary>
/// Orchestrates cube operation workflows.
/// </summary>
public class Orchestrator(ICubeService cubeService) : IOrchestrator
{
    public MoveSequence GetChallengeSequence() 
        => MoveSequence.CreateChallengeSequence();

    public void ExecuteChallenge()
    {
        var sequence = GetChallengeSequence();
        cubeService.ExecuteSequence(sequence);
    }

    public void ExecuteChallengeStepByStep(Action<Move, int> onMoveExecuted)
    {
        var sequence = GetChallengeSequence();
        int step = 1;

        foreach (var move in sequence)
        {
            cubeService.ApplyMove(move);
            onMoveExecuted(move, step++);
        }
    }
}
