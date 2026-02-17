namespace RubiksCube.Infrastructure.Rendering;

/// <summary>
/// Renders a Rubik's Cube to the console in exploded view format.
/// </summary>
public class ConsoleCubeRenderer : ICubeRenderer
{
    private const string Separator = "   ";
    private const int Padding = 9; // Align with FRONT face position

    public void Render(Cube cube) => Render(cube, null);

    public void Render(Cube cube, string? title)
    {
        if (!string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine();
            Console.WriteLine(title);
            Console.WriteLine(new string('=', title.Length));
        }

        Console.WriteLine();

        // Up (centered top)
        RenderFace(cube, FaceType.Up, "UP", centered: true);
        Console.WriteLine();

        // Middle row: Left, Front, Right, Back
        RenderMiddleRow(cube);
        Console.WriteLine();

        // Down (centered bottom)
        RenderFace(cube, FaceType.Down, "DOWN", centered: true);
        Console.WriteLine();
    }

    private static void RenderMiddleRow(Cube cube)
    {
        var faces = new[]
        {
            (FaceType.Left, "LEFT"),
            (FaceType.Front, "FRONT"),
            (FaceType.Right, "RIGHT"),
            (FaceType.Back, "BACK")
        };

        // Labels (aligned to match 6-char tile width)
        foreach (var (_, label) in faces)
        {
            Console.Write(label == "BACK" ? $"{label,-6}" : $"{label,-6}{Separator}");
        }
        Console.WriteLine();

        // Rows
        for (int row = 0; row < 3; row++)
        {
            foreach (var (faceType, _) in faces)
            {
                var face = cube.GetFace(faceType);
                RenderRow(face, row);
                if (faceType != FaceType.Back) Console.Write(Separator);
            }
            Console.WriteLine();
        }
    }

    private static void RenderFace(Cube cube, FaceType faceType, string label, bool centered)
    {
        var face = cube.GetFace(faceType);

        if (centered)
        {
            Console.WriteLine($"{new string(' ', Padding)}{label}");
            for (int row = 0; row < 3; row++)
            {
                Console.Write(new string(' ', Padding));
                RenderRow(face, row);
                Console.WriteLine();
            }
        }
        else
        {
            Console.WriteLine(label);
            for (int row = 0; row < 3; row++)
            {
                RenderRow(face, row);
                Console.WriteLine();
            }
        }
    }

    private static void RenderRow(ColorType[,] face, int row)
    {
        for (int col = 0; col < 3; col++)
        {
            var color = face[row, col];
            var originalColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColorHelper.GetConsoleColor(color);
            Console.Write($"{ConsoleColorHelper.GetColorChar(color)} ");
            Console.ForegroundColor = originalColor;
        }
    }
}
