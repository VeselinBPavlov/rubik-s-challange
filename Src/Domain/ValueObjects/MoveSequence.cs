namespace RubiksCube.Domain.ValueObjects;

using System.Collections;
using System.Collections.ObjectModel;

/// <summary>
/// Represents an immutable sequence of moves.
/// </summary>
public sealed class MoveSequence : ValueObject, IEnumerable<Move>
{
    private readonly ReadOnlyCollection<Move> _moves;

    public MoveSequence(IEnumerable<Move> moves)
    {
        ArgumentNullException.ThrowIfNull(moves);
        _moves = moves.ToList().AsReadOnly();
    }

    /// <summary>
    /// Parses a space-separated notation string (e.g. "F R' U B' L D'") into a sequence.
    /// </summary>
    /// <exception cref="InvalidMoveNotationException">Thrown when any token is invalid.</exception>
    public static MoveSequence FromNotation(string notation)
    {
        ArgumentNullException.ThrowIfNull(notation);

        var tokens = notation.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var moves = tokens.Select(Move.FromNotation).ToList();
        return new MoveSequence(moves);
    }

    /// <summary>
    /// Creates the challenge sequence: F R' U B' L D'.
    /// </summary>
    public static MoveSequence CreateChallengeSequence()
        => new([
            Move.Clockwise(FaceType.Front),
            Move.AntiClockwise(FaceType.Right),
            Move.Clockwise(FaceType.Up),
            Move.AntiClockwise(FaceType.Back),
            Move.Clockwise(FaceType.Left),
            Move.AntiClockwise(FaceType.Down)
        ]);

    /// <summary>
    /// Returns the sequence in standard notation (e.g. "F R' U B' L D'").
    /// </summary>
    public override string ToString() =>
        string.Join(" ", _moves.Select(m => m.ToString()));

    /// <summary>
    /// Gets a detailed, multi-line description of every move in the sequence.
    /// </summary>
    public string GetDetailedDescription() =>
        string.Join(Environment.NewLine,
            _moves.Select((move, index) => $"{index + 1}. {move.GetDescription()}"));

    public IEnumerator<Move> GetEnumerator() => _moves.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    protected override IEnumerable<object> GetAtomicValues()
    {
        foreach (var move in _moves)
            yield return move;
    }
}
