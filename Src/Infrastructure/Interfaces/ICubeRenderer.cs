namespace RubiksCube.Infrastructure.Interfaces;

/// <summary>
/// Interface for rendering the Rubik's Cube to an output medium.
/// Allows different rendering implementations (console, GUI, web, etc.)
/// </summary>
public interface ICubeRenderer
{
    /// <summary>
    /// Renders the cube in its current state
    /// </summary>
    /// <param name="cube">The cube to render</param>
    void Render(Cube cube);

    /// <summary>
    /// Renders the cube with a title
    /// </summary>
    /// <param name="cube">The cube to render</param>
    /// <param name="title">Title to display above the cube</param>
    void Render(Cube cube, string title);
}
