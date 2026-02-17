namespace RubiksCube.Tests.Domain.ValueObjects;

public class MoveTests
{
    [Theory]
    [InlineData(FaceType.Front, RotationDirection.Clockwise)]
    [InlineData(FaceType.Back, RotationDirection.AntiClockwise)]
    [InlineData(FaceType.Left, RotationDirection.Clockwise)]
    [InlineData(FaceType.Right, RotationDirection.AntiClockwise)]
    [InlineData(FaceType.Up, RotationDirection.Clockwise)]
    [InlineData(FaceType.Down, RotationDirection.AntiClockwise)]
    public void Constructor_WithValidParameters_ShouldCreateMove(FaceType face, RotationDirection direction)
    {
        // Act
        var move = new Move(face, direction);

        // Assert
        move.Face.Should().Be(face);
        move.Direction.Should().Be(direction);
    }

    [Fact]
    public void Constructor_WithInvalidFace_ShouldThrowArgumentException()
    {
        // Arrange
        var invalidFace = (FaceType)999;

        // Act
        var act = () => new Move(invalidFace, RotationDirection.Clockwise);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithParameterName("face");
    }

    [Fact]
    public void Constructor_WithInvalidDirection_ShouldThrowArgumentException()
    {
        // Arrange
        var invalidDirection = (RotationDirection)999;

        // Act
        var act = () => new Move(FaceType.Front, invalidDirection);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithParameterName("direction");
    }

    [Theory]
    [InlineData(FaceType.Front, RotationDirection.Clockwise, "F")]
    [InlineData(FaceType.Front, RotationDirection.AntiClockwise, "F'")]
    [InlineData(FaceType.Back, RotationDirection.Clockwise, "B")]
    [InlineData(FaceType.Back, RotationDirection.AntiClockwise, "B'")]
    [InlineData(FaceType.Left, RotationDirection.Clockwise, "L")]
    [InlineData(FaceType.Left, RotationDirection.AntiClockwise, "L'")]
    [InlineData(FaceType.Right, RotationDirection.Clockwise, "R")]
    [InlineData(FaceType.Right, RotationDirection.AntiClockwise, "R'")]
    [InlineData(FaceType.Up, RotationDirection.Clockwise, "U")]
    [InlineData(FaceType.Up, RotationDirection.AntiClockwise, "U'")]
    [InlineData(FaceType.Down, RotationDirection.Clockwise, "D")]
    [InlineData(FaceType.Down, RotationDirection.AntiClockwise, "D'")]
    public void ToString_ShouldReturnCorrectNotation(FaceType face, RotationDirection direction, string expected)
    {
        // Arrange
        var move = new Move(face, direction);

        // Act
        var notation = move.ToString();

        // Assert
        notation.Should().Be(expected);
    }

    [Theory]
    [InlineData(FaceType.Front, RotationDirection.Clockwise, "Front face clockwise 90°")]
    [InlineData(FaceType.Right, RotationDirection.AntiClockwise, "Right face anti-clockwise 90°")]
    [InlineData(FaceType.Up, RotationDirection.Clockwise, "Up face clockwise 90°")]
    public void GetDescription_ShouldReturnHumanReadableText(FaceType face, RotationDirection direction, string expected)
    {
        // Arrange
        var move = new Move(face, direction);

        // Act
        var description = move.GetDescription();

        // Assert
        description.Should().Be(expected);
    }

    [Fact]
    public void Clockwise_ShouldCreateClockwiseMove()
    {
        // Act
        var move = Move.Clockwise(FaceType.Front);

        // Assert
        move.Face.Should().Be(FaceType.Front);
        move.Direction.Should().Be(RotationDirection.Clockwise);
        move.ToString().Should().Be("F");
    }

    [Fact]
    public void AntiClockwise_ShouldCreateAntiClockwiseMove()
    {
        // Act
        var move = Move.AntiClockwise(FaceType.Right);

        // Assert
        move.Face.Should().Be(FaceType.Right);
        move.Direction.Should().Be(RotationDirection.AntiClockwise);
        move.ToString().Should().Be("R'");
    }

    #region FromNotation

    [Theory]
    [InlineData("F",  FaceType.Front, RotationDirection.Clockwise)]
    [InlineData("F'", FaceType.Front, RotationDirection.AntiClockwise)]
    [InlineData("R",  FaceType.Right, RotationDirection.Clockwise)]
    [InlineData("R'", FaceType.Right, RotationDirection.AntiClockwise)]
    [InlineData("U",  FaceType.Up,    RotationDirection.Clockwise)]
    [InlineData("U'", FaceType.Up,    RotationDirection.AntiClockwise)]
    [InlineData("B",  FaceType.Back,  RotationDirection.Clockwise)]
    [InlineData("B'", FaceType.Back,  RotationDirection.AntiClockwise)]
    [InlineData("L",  FaceType.Left,  RotationDirection.Clockwise)]
    [InlineData("L'", FaceType.Left,  RotationDirection.AntiClockwise)]
    [InlineData("D",  FaceType.Down,  RotationDirection.Clockwise)]
    [InlineData("D'", FaceType.Down,  RotationDirection.AntiClockwise)]
    public void FromNotation_WithValidNotation_ShouldReturnCorrectMove(
        string notation, FaceType expectedFace, RotationDirection expectedDir)
    {
        // Act
        var move = Move.FromNotation(notation);

        // Assert
        move.Face.Should().Be(expectedFace);
        move.Direction.Should().Be(expectedDir);
    }

    [Theory]
    [InlineData("X")]
    [InlineData("Z")]
    [InlineData("")]
    [InlineData("FF")]
    public void FromNotation_WithInvalidNotation_ShouldThrowInvalidMoveNotationException(string notation)
    {
        // Act
        var act = () => Move.FromNotation(notation);

        // Assert
        act.Should().Throw<InvalidMoveNotationException>();
    }

    [Fact]
    public void FromNotation_WithNull_ShouldThrowArgumentNullException()
    {
        var act = () => Move.FromNotation(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region Inverse

    [Fact]
    public void Inverse_ClockwiseMove_ShouldReturnAntiClockwise()
    {
        var move = Move.Clockwise(FaceType.Front);
        var inverse = move.Inverse();

        inverse.Face.Should().Be(FaceType.Front);
        inverse.Direction.Should().Be(RotationDirection.AntiClockwise);
    }

    [Fact]
    public void Inverse_AntiClockwiseMove_ShouldReturnClockwise()
    {
        var move = Move.AntiClockwise(FaceType.Right);
        var inverse = move.Inverse();

        inverse.Face.Should().Be(FaceType.Right);
        inverse.Direction.Should().Be(RotationDirection.Clockwise);
    }

    #endregion

    #region Equality

    [Fact]
    public void Equals_SameFaceAndDirection_ShouldBeTrue()
    {
        var a = Move.Clockwise(FaceType.Front);
        var b = Move.Clockwise(FaceType.Front);

        a.Equals(b).Should().BeTrue();
        (a == b).Should().BeTrue();
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void Equals_DifferentDirection_ShouldBeFalse()
    {
        var a = Move.Clockwise(FaceType.Front);
        var b = Move.AntiClockwise(FaceType.Front);

        a.Equals(b).Should().BeFalse();
        (a != b).Should().BeTrue();
    }

    [Fact]
    public void Equals_DifferentFace_ShouldBeFalse()
    {
        var a = Move.Clockwise(FaceType.Front);
        var b = Move.Clockwise(FaceType.Back);

        a.Equals(b).Should().BeFalse();
    }

    [Fact]
    public void Equals_Null_ShouldBeFalse()
    {
        var move = Move.Clockwise(FaceType.Front);

        move.Equals(null).Should().BeFalse();
        (move == null).Should().BeFalse();
    }

    #endregion
}
