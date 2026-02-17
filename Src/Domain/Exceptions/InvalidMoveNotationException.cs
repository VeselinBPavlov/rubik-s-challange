namespace RubiksCube.Domain.Exceptions;

/// <summary>
/// Thrown when an invalid Rubik's Cube move notation string is encountered.
/// </summary>
public sealed class InvalidMoveNotationException : DomainException
{
    /// <summary>
    /// The notation string that was invalid.
    /// </summary>
    public string Notation { get; }

    public InvalidMoveNotationException(string notation)
        : base($"Invalid move notation: '{notation}'. Valid format: F, F', R, R', U, U', B, B', L, L', D, D'")
    {
        Notation = notation;
    }
}
