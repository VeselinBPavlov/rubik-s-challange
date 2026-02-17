namespace RubiksCube.Tests.Application.Services;

public class CubeServiceTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithProvidedCube()
    {
        // Arrange
        var cube = Cube.CreateSolved();

        // Act
        var service = new CubeService(cube);

        // Assert
        service.Cube.Should().BeSameAs(cube);
    }

    [Fact]
    public void ApplyMove_ShouldApplyMoveToInternalCube()
    {
        // Arrange
        var cube = Cube.CreateSolved();
        var service = new CubeService(cube);
        var move = Move.Clockwise(FaceType.Front);

        // Act
        service.ApplyMove(move);

        // Assert
        var upFace = service.Cube.GetFace(FaceType.Up);
        upFace[2, 0].Should().Be(ColorType.Orange);
    }

    [Fact]
    public void ExecuteSequence_ShouldApplyAllMovesInOrder()
    {
        // Arrange
        var cube = Cube.CreateSolved();
        var service = new CubeService(cube);
        var moves = new[]
        {
            Move.Clockwise(FaceType.Front),
            Move.AntiClockwise(FaceType.Right),
            Move.Clockwise(FaceType.Up)
        };
        var sequence = new MoveSequence(moves);

        // Act
        service.ExecuteSequence(sequence);

        // Assert
        var frontFace = service.Cube.GetFace(FaceType.Front);
        frontFace.Should().NotBeEquivalentTo(CreateUniformFace(ColorType.Green));
    }

    [Fact]
    public void ExecuteSequence_WithChallengeSequence_ShouldModifyCube()
    {
        // Arrange
        var cube = Cube.CreateSolved();
        var service = new CubeService(cube);
        var sequence = MoveSequence.CreateChallengeSequence();

        // Act
        service.ExecuteSequence(sequence);

        // Assert - Verify cube is scrambled
        var faces = new[] { FaceType.Front, FaceType.Back, FaceType.Left, FaceType.Right, FaceType.Up, FaceType.Down };
        var solvedStates = faces.Select(f => IsFaceUniform(service.Cube, f)).ToList();
        solvedStates.Should().Contain(false, "at least one face should be scrambled");
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

    private static bool IsFaceUniform(Cube cube, FaceType faceType)
    {
        var face = cube.GetFace(faceType);
        var firstColor = face[0, 0];
        
        for (int r = 0; r < 3; r++)
            for (int c = 0; c < 3; c++)
                if (face[r, c] != firstColor)
                    return false;
        
        return true;
    }
}
