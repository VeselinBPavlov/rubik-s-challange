namespace RubiksCube.Tests.Application.Services;

public class OrchestratorTests
{
    [Fact]
    public void GetChallengeSequence_ShouldReturnPredefinedSequence()
    {
        // Arrange
        var cube = Cube.CreateSolved();
        var cubeService = new CubeService(cube);
        var orchestrator = new Orchestrator(cubeService);

        // Act
        var sequence = orchestrator.GetChallengeSequence();

        // Assert
        sequence.Should().NotBeNull();
        sequence.ToString().Should().Be("F R' U B' L D'");
    }

    [Fact]
    public void ExecuteChallenge_ShouldApplyAllMoves()
    {
        // Arrange
        var cube = Cube.CreateSolved();
        var cubeService = new CubeService(cube);
        var orchestrator = new Orchestrator(cubeService);

        // Act
        orchestrator.ExecuteChallenge();

        // Assert
        var frontFace = cubeService.Cube.GetFace(FaceType.Front);
        frontFace.Should().NotBeEquivalentTo(CreateUniformFace(ColorType.Green));
    }

    [Fact]
    public void ExecuteChallengeStepByStep_ShouldCallCallbackForEachMove()
    {
        // Arrange
        var cube = Cube.CreateSolved();
        var cubeService = new CubeService(cube);
        var orchestrator = new Orchestrator(cubeService);
        var callbackCount = 0;
        var steps = new List<int>();
        var moves = new List<string>();

        // Act
        orchestrator.ExecuteChallengeStepByStep((move, step) =>
        {
            callbackCount++;
            steps.Add(step);
            moves.Add(move.ToString());
        });

        // Assert
        callbackCount.Should().Be(6);
        steps.Should().Equal(1, 2, 3, 4, 5, 6);
        moves.Should().Equal("F", "R'", "U", "B'", "L", "D'");
    }

    [Fact]
    public void ExecuteChallengeStepByStep_ShouldApplyMovesInCorrectOrder()
    {
        // Arrange
        var cube = Cube.CreateSolved();
        var cubeService = new CubeService(cube);
        var orchestrator = new Orchestrator(cubeService);
        var moveDescriptions = new List<string>();

        // Act
        orchestrator.ExecuteChallengeStepByStep((move, step) =>
        {
            moveDescriptions.Add(move.GetDescription());
        });

        // Assert
        moveDescriptions[0].Should().Contain("Front face clockwise");
        moveDescriptions[1].Should().Contain("Right face anti-clockwise");
        moveDescriptions[2].Should().Contain("Up face clockwise");
        moveDescriptions[3].Should().Contain("Back face anti-clockwise");
        moveDescriptions[4].Should().Contain("Left face clockwise");
        moveDescriptions[5].Should().Contain("Down face anti-clockwise");
    }

    // Helper Methods
    private static ColorType[,] CreateUniformFace(ColorType color)
    {
        var face = new ColorType[3, 3];
        for (int r = 0; r < 3; r++)
            for (int c = 0; c < 3; c++)
                face[r, c] = color;
        return face;
    }
}
