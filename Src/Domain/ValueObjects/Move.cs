using RubiksCube.Domain.Exceptions;

namespace RubiksCube.Domain.ValueObjects;

/// <summary>
/// Represents a single move on a Rubik's Cube (immutable value object).
/// Supports standard Rubik's Cube notation (F, R', U, etc.).
/// </summary>
public sealed class Move : ValueObject
{
    private static readonly Dictionary<string, (FaceType Face, RotationDirection Dir)> NotationMap = new()
    {
        ["F"]  = (FaceType.Front, RotationDirection.Clockwise),
        ["F'"] = (FaceType.Front, RotationDirection.AntiClockwise),
        ["R"]  = (FaceType.Right, RotationDirection.Clockwise),
        ["R'"] = (FaceType.Right, RotationDirection.AntiClockwise),
        ["U"]  = (FaceType.Up, RotationDirection.Clockwise),
        ["U'"] = (FaceType.Up, RotationDirection.AntiClockwise),
        ["B"]  = (FaceType.Back, RotationDirection.Clockwise),
        ["B'"] = (FaceType.Back, RotationDirection.AntiClockwise),
        ["L"]  = (FaceType.Left, RotationDirection.Clockwise),
        ["L'"] = (FaceType.Left, RotationDirection.AntiClockwise),
        ["D"]  = (FaceType.Down, RotationDirection.Clockwise),
        ["D'"] = (FaceType.Down, RotationDirection.AntiClockwise),
    };

    public FaceType Face { get; }

    public RotationDirection Direction { get; }

    public Move(FaceType face, RotationDirection direction)
    {
        if (!Enum.IsDefined(face))
            throw new ArgumentException("Invalid face type", nameof(face));

        if (!Enum.IsDefined(direction))
            throw new ArgumentException("Invalid rotation direction", nameof(direction));

        Face = face;
        Direction = direction;
    }

    /// <summary>
    /// Parses standard notation (F, R', U, B', L, D') into a Move.
    /// </summary>
    /// <exception cref="InvalidMoveNotationException">Thrown when the notation is not recognised.</exception>
    public static Move FromNotation(string notation)
    {
        ArgumentNullException.ThrowIfNull(notation);

        var trimmed = notation.Trim();

        if (NotationMap.TryGetValue(trimmed, out var entry))
            return new Move(entry.Face, entry.Dir);

        throw new InvalidMoveNotationException(trimmed);
    }

    /// <summary>
    /// Returns move in standard notation (F, R', U, etc.).
    /// </summary>
    public override string ToString()
    {
        var faceChar = Face switch
        {
            FaceType.Front => "F",
            FaceType.Back => "B",
            FaceType.Left => "L",
            FaceType.Right => "R",
            FaceType.Up => "U",
            FaceType.Down => "D",
            _ => throw new InvalidOperationException()
        };

        return Direction == RotationDirection.Clockwise
            ? faceChar
            : $"{faceChar}'";
    }

    /// <summary>
    /// Gets a human-readable description of the move.
    /// </summary>
    public string GetDescription()
    {
        var directionText = Direction == RotationDirection.Clockwise
            ? "clockwise"
            : "anti-clockwise";

        return $"{Face} face {directionText} 90°";
    }

    /// <summary>
    /// Returns the inverse of this move (clockwise becomes anti-clockwise and vice versa).
    /// </summary>
    public Move Inverse() => new(Face,
        Direction == RotationDirection.Clockwise
            ? RotationDirection.AntiClockwise
            : RotationDirection.Clockwise);

    /// <summary>
    /// Factory method for clockwise rotation.
    /// </summary>
    public static Move Clockwise(FaceType face) => new(face, RotationDirection.Clockwise);

    /// <summary>
    /// Factory method for anti-clockwise rotation.
    /// </summary>
    public static Move AntiClockwise(FaceType face) => new(face, RotationDirection.AntiClockwise);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Face;
        yield return Direction;
    }
}

