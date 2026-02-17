namespace RubiksCube.Infrastructure.Rendering;

/// <summary>
/// Helper class for console color and character operations
/// </summary>
public static class ConsoleColorHelper
{
    /// <summary>
    /// Gets the console foreground color for a cube color
    /// </summary>
    public static ConsoleColor GetConsoleColor(ColorType color)
    {
        return color switch
        {
            ColorType.White => ConsoleColor.White,
            ColorType.Yellow => ConsoleColor.Yellow,
            ColorType.Green => ConsoleColor.Green,
            ColorType.Blue => ConsoleColor.Blue,
            ColorType.Red => ConsoleColor.Red,
            ColorType.Orange => ConsoleColor.DarkYellow,
            _ => ConsoleColor.Gray
        };
    }

    /// <summary>
    /// Gets the character representation for a cube color
    /// </summary>
    public static char GetColorChar(ColorType color)
    {
        return color switch
        {
            ColorType.White => 'W',
            ColorType.Yellow => 'Y',
            ColorType.Green => 'G',
            ColorType.Blue => 'B',
            ColorType.Red => 'R',
            ColorType.Orange => 'O',
            _ => '?'
        };
    }
}
