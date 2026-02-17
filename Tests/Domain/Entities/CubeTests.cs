namespace RubiksCube.Tests.Domain.Entities;

public class CubeTests
{
    [Fact]
    public void CreateSolved_ShouldInitializeCubeWithCorrectColors()
    {
        // Arrange & Act
        var cube = Cube.CreateSolved();

        // Assert
        AssertFaceColor(cube, FaceType.Front, ColorType.Green);
        AssertFaceColor(cube, FaceType.Right, ColorType.Red);
        AssertFaceColor(cube, FaceType.Up, ColorType.White);
        AssertFaceColor(cube, FaceType.Back, ColorType.Blue);
        AssertFaceColor(cube, FaceType.Left, ColorType.Orange);
        AssertFaceColor(cube, FaceType.Down, ColorType.Yellow);
    }

    [Fact]
    public void GetFace_ShouldReturnCopyNotReference()
    {
        // Arrange
        var cube = Cube.CreateSolved();
        var face1 = cube.GetFace(FaceType.Front);
        
        // Act
        face1[0, 0] = ColorType.Red; // Modify the copy
        var face2 = cube.GetFace(FaceType.Front);

        // Assert
        face2[0, 0].Should().Be(ColorType.Green, "original cube should not be modified");
    }

    [Theory]
    [InlineData(FaceType.Front)]
    [InlineData(FaceType.Back)]
    [InlineData(FaceType.Left)]
    [InlineData(FaceType.Right)]
    [InlineData(FaceType.Up)]
    [InlineData(FaceType.Down)]
    public void RotateFaceClockwise_ThenCounterClockwise_ShouldReturnToOriginalState(FaceType face)
    {
        // Arrange
        var cube = Cube.CreateSolved();
        var originalFace = cube.GetFace(face);

        // Act
        cube.ApplyMove(Move.Clockwise(face));
        cube.ApplyMove(Move.AntiClockwise(face));
        var finalFace = cube.GetFace(face);

        // Assert
        finalFace.Should().BeEquivalentTo(originalFace);
    }

    [Fact]
    public void RotateFrontClockwise_ShouldRotateFaceStickersCorrectly()
    {
        // Arrange
        var cube = Cube.CreateSolved();

        // Act
        cube.ApplyMove(Move.Clockwise(FaceType.Front));
        var face = cube.GetFace(FaceType.Front);

        // Assert - All stickers should still be green
        for (int r = 0; r < 3; r++)
            for (int c = 0; c < 3; c++)
                face[r, c].Should().Be(ColorType.Green);
    }

    [Fact]
    public void RotateFrontClockwise_ShouldMoveEdgePiecesCorrectly()
    {
        // Arrange
        var cube = Cube.CreateSolved();

        // Act
        cube.ApplyMove(Move.Clockwise(FaceType.Front));

        // Assert - Check edge movements
        // Up bottom row -> Right left column
        var rightFace = cube.GetFace(FaceType.Right);
        rightFace[0, 0].Should().Be(ColorType.White);
        rightFace[1, 0].Should().Be(ColorType.White);
        rightFace[2, 0].Should().Be(ColorType.White);

        // Right left column -> Down top row
        var downFace = cube.GetFace(FaceType.Down);
        downFace[0, 2].Should().Be(ColorType.Red);
        downFace[0, 1].Should().Be(ColorType.Red);
        downFace[0, 0].Should().Be(ColorType.Red);

        // Down top row -> Left right column
        var leftFace = cube.GetFace(FaceType.Left);
        leftFace[0, 2].Should().Be(ColorType.Yellow);
        leftFace[1, 2].Should().Be(ColorType.Yellow);
        leftFace[2, 2].Should().Be(ColorType.Yellow);

        // Left right column -> Up bottom row
        var upFace = cube.GetFace(FaceType.Up);
        upFace[2, 2].Should().Be(ColorType.Orange);
        upFace[2, 1].Should().Be(ColorType.Orange);
        upFace[2, 0].Should().Be(ColorType.Orange);
    }

    [Fact]
    public void ApplyMove_FourClockwiseRotations_ShouldReturnToOriginalState()
    {
        // Arrange
        var cube = Cube.CreateSolved();
        var move = Move.Clockwise(FaceType.Right);

        // Act
        cube.ApplyMove(move);
        cube.ApplyMove(move);
        cube.ApplyMove(move);
        cube.ApplyMove(move);

        // Assert
        AssertFaceColor(cube, FaceType.Front, ColorType.Green);
        AssertFaceColor(cube, FaceType.Right, ColorType.Red);
        AssertFaceColor(cube, FaceType.Up, ColorType.White);
        AssertFaceColor(cube, FaceType.Back, ColorType.Blue);
        AssertFaceColor(cube, FaceType.Left, ColorType.Orange);
        AssertFaceColor(cube, FaceType.Down, ColorType.Yellow);
    }

    [Fact]
    public void ExecuteChallengeSequence_ShouldProduceExactExpectedFinalState()
    {
        // Arrange
        var cube = Cube.CreateSolved();
        var sequence = MoveSequence.CreateChallengeSequence();

        // Act
        foreach (var move in sequence)
            cube.ApplyMove(move);

        // Assert — verified cell-by-cell against rubiks-cube-solver.com
        // UP face
        var up = cube.GetFace(FaceType.Up);
        up.Should().BeEquivalentTo(new ColorType[,]
        {
            { ColorType.Red,   ColorType.Orange, ColorType.Green },
            { ColorType.Blue,  ColorType.White,  ColorType.White },
            { ColorType.Blue,  ColorType.Blue,   ColorType.Blue  },
        });

        // DOWN face
        var down = cube.GetFace(FaceType.Down);
        down.Should().BeEquivalentTo(new ColorType[,]
        {
            { ColorType.Green, ColorType.Green,  ColorType.Blue  },
            { ColorType.Red,   ColorType.Yellow, ColorType.Red   },
            { ColorType.Red,   ColorType.Green,  ColorType.Green },
        });

        // FRONT face
        var front = cube.GetFace(FaceType.Front);
        front.Should().BeEquivalentTo(new ColorType[,]
        {
            { ColorType.Orange, ColorType.Red,   ColorType.Red   },
            { ColorType.Orange, ColorType.Green,  ColorType.White },
            { ColorType.White,  ColorType.White,  ColorType.White },
        });

        // BACK face
        var back = cube.GetFace(FaceType.Back);
        back.Should().BeEquivalentTo(new ColorType[,]
        {
            { ColorType.Yellow, ColorType.Blue,   ColorType.White  },
            { ColorType.Orange, ColorType.Blue,   ColorType.Yellow },
            { ColorType.Yellow, ColorType.Yellow,  ColorType.White },
        });

        // LEFT face
        var left = cube.GetFace(FaceType.Left);
        left.Should().BeEquivalentTo(new ColorType[,]
        {
            { ColorType.Green,  ColorType.Yellow, ColorType.Yellow },
            { ColorType.Orange, ColorType.Orange, ColorType.Green  },
            { ColorType.Blue,   ColorType.Green,  ColorType.Orange },
        });

        // RIGHT face
        var right = cube.GetFace(FaceType.Right);
        right.Should().BeEquivalentTo(new ColorType[,]
        {
            { ColorType.Yellow, ColorType.Blue,   ColorType.Orange },
            { ColorType.Red,    ColorType.Red,    ColorType.White  },
            { ColorType.Orange, ColorType.Yellow,  ColorType.Red  },
        });
    }

    [Fact]
    public void RotateUpClockwise_ShouldRotateTopRowOfSurroundingFaces()
    {
        // Arrange
        var cube = Cube.CreateSolved();

        // Act
        cube.ApplyMove(Move.Clockwise(FaceType.Up));

        // Assert
        var frontFace = cube.GetFace(FaceType.Front);
        frontFace[0, 0].Should().Be(ColorType.Red);
        frontFace[0, 1].Should().Be(ColorType.Red);
        frontFace[0, 2].Should().Be(ColorType.Red);

        var leftFace = cube.GetFace(FaceType.Left);
        leftFace[0, 0].Should().Be(ColorType.Green);
        leftFace[0, 1].Should().Be(ColorType.Green);
        leftFace[0, 2].Should().Be(ColorType.Green);
    }

    [Fact]
    public void RotateRightAntiClockwise_ShouldMoveEdgePiecesCorrectly()
    {
        // Arrange
        var cube = Cube.CreateSolved();

        // Act
        cube.ApplyMove(Move.AntiClockwise(FaceType.Right));

        // Assert
        var frontFace = cube.GetFace(FaceType.Front);
        frontFace[0, 2].Should().Be(ColorType.White);
        frontFace[1, 2].Should().Be(ColorType.White);
        frontFace[2, 2].Should().Be(ColorType.White);

        var downFace = cube.GetFace(FaceType.Down);
        downFace[0, 2].Should().Be(ColorType.Green);
        downFace[1, 2].Should().Be(ColorType.Green);
        downFace[2, 2].Should().Be(ColorType.Green);
    }

    [Fact]
    public void ApplyMove_WithNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var cube = Cube.CreateSolved();

        // Act
        var act = () => cube.ApplyMove(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    // Helper Methods
    private static void AssertFaceColor(Cube cube, FaceType faceType, ColorType expectedColor)
    {
        var face = cube.GetFace(faceType);
        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                face[r, c].Should().Be(expectedColor, 
                    $"{faceType} face should be uniformly {expectedColor} at position [{r},{c}]");
            }
        }
    }
}
