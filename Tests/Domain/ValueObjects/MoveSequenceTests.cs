namespace RubiksCube.Tests.Domain.ValueObjects;

public class MoveSequenceTests
{
    [Fact]
    public void Constructor_WithValidMoves_ShouldCreateSequence()
    {
        // Arrange
        var moves = new[]
        {
            Move.Clockwise(FaceType.Front),
            Move.AntiClockwise(FaceType.Right)
        };

        // Act
        var sequence = new MoveSequence(moves);

        // Assert
        sequence.Should().NotBeNull();
        sequence.Count().Should().Be(2);
    }

    [Fact]
    public void CreateChallengeSequence_ShouldCreateCorrectSequence()
    {
        // Act
        var sequence = MoveSequence.CreateChallengeSequence();

        // Assert
        sequence.Count().Should().Be(6);
        sequence.ToString().Should().Be("F R' U B' L D'");
    }

    [Fact]
    public void ToString_ShouldReturnStandardNotation()
    {
        // Arrange
        var moves = new[]
        {
            Move.Clockwise(FaceType.Front),
            Move.AntiClockwise(FaceType.Right),
            Move.Clockwise(FaceType.Up)
        };
        var sequence = new MoveSequence(moves);

        // Act
        var notation = sequence.ToString();

        // Assert
        notation.Should().Be("F R' U");
    }

    [Fact]
    public void GetDetailedDescription_ShouldReturnMultilineDescription()
    {
        // Arrange
        var moves = new[]
        {
            Move.Clockwise(FaceType.Front),
            Move.AntiClockwise(FaceType.Right)
        };
        var sequence = new MoveSequence(moves);

        // Act
        var description = sequence.GetDetailedDescription();

        // Assert
        description.Should().Contain("1. Front face clockwise 90°");
        description.Should().Contain("2. Right face anti-clockwise 90°");
    }

    [Fact]
    public void GetEnumerator_ShouldAllowIteration()
    {
        // Arrange
        var moves = new[]
        {
            Move.Clockwise(FaceType.Front),
            Move.AntiClockwise(FaceType.Right),
            Move.Clockwise(FaceType.Up)
        };
        var sequence = new MoveSequence(moves);

        // Act
        var movesList = new List<Move>();
        foreach (var move in sequence)
        {
            movesList.Add(move);
        }

        // Assert
        movesList.Should().HaveCount(3);
        movesList[0].ToString().Should().Be("F");
        movesList[1].ToString().Should().Be("R'");
        movesList[2].ToString().Should().Be("U");
    }

    [Fact]
    public void MoveSequence_ShouldBeImmutable()
    {
        // Arrange
        var moves = new List<Move>
        {
            Move.Clockwise(FaceType.Front),
            Move.AntiClockwise(FaceType.Right)
        };
        var sequence = new MoveSequence(moves);
        var originalCount = sequence.Count();

        // Act - Try to modify original list
        moves.Add(Move.Clockwise(FaceType.Up));

        // Assert - Sequence should not change
        sequence.Count().Should().Be(originalCount);
    }

    #region FromNotation

    [Fact]
    public void FromNotation_WithValidString_ShouldCreateCorrectSequence()
    {
        // Act
        var sequence = MoveSequence.FromNotation("F R' U B' L D'");

        // Assert
        sequence.Count().Should().Be(6);
        sequence.ToString().Should().Be("F R' U B' L D'");
    }

    [Fact]
    public void FromNotation_WithSingleMove_ShouldCreateSingleElementSequence()
    {
        var sequence = MoveSequence.FromNotation("R");

        sequence.Count().Should().Be(1);
        sequence.First().Should().Be(Move.Clockwise(FaceType.Right));
    }

    [Fact]
    public void FromNotation_WithInvalidToken_ShouldThrowInvalidMoveNotationException()
    {
        var act = () => MoveSequence.FromNotation("F X U");

        act.Should().Throw<InvalidMoveNotationException>();
    }

    [Fact]
    public void FromNotation_WithNull_ShouldThrowArgumentNullException()
    {
        var act = () => MoveSequence.FromNotation(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    #endregion
}
