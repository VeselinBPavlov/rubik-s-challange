namespace RubiksCube.Domain.Entities;

public class Cube
{
    // 6 faces × 3×3 stickers
    private readonly ColorType[,,] _faces = new ColorType[6, 3, 3];

    private Cube() { }

    public static Cube CreateSolved()
    {
        var cube = new Cube();

        foreach (FaceType face in Enum.GetValues<FaceType>())
        {
            var color = face switch
            {
                FaceType.Front => ColorType.Green,
                FaceType.Right => ColorType.Red,
                FaceType.Up => ColorType.White,
                FaceType.Left => ColorType.Orange,
                FaceType.Down => ColorType.Yellow,
                FaceType.Back => ColorType.Blue,
                _ => throw new ArgumentOutOfRangeException()
            };

            for (int r = 0; r < 3; r++)
                for (int c = 0; c < 3; c++)
                    cube._faces[(int)face, r, c] = color;
        }

        return cube;
    }

    /// <summary>
    /// Returns a copy of the 3×3 face.
    /// </summary>
    public ColorType[,] GetFace(FaceType face)
    {
        var result = new ColorType[3, 3];

        for (int r = 0; r < 3; r++)
            for (int c = 0; c < 3; c++)
                result[r, c] = _faces[(int)face, r, c];

        return result;
    }

    /// <summary>
    /// Applies a move to the cube.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="move"/> is null.</exception>
    public void ApplyMove(Move move)
    {
        ArgumentNullException.ThrowIfNull(move);
        switch (move.Face)
        {
            case FaceType.Front:
                RotateFront(move.Direction);
                break;
            case FaceType.Back:
                RotateBack(move.Direction);
                break;
            case FaceType.Left:
                RotateLeft(move.Direction);
                break;
            case FaceType.Right:
                RotateRight(move.Direction);
                break;
            case FaceType.Up:
                RotateUp(move.Direction);
                break;
            case FaceType.Down:
                RotateDown(move.Direction);
                break;
        }
    }

    #region Face Rotations

    private void RotateFront(RotationDirection direction)
    {
        RotateFaceStickers(FaceType.Front, direction);

        var upBottom = GetRow(FaceType.Up, 2);
        var rightLeft = GetColumn(FaceType.Right, 0);
        var downTop = GetRow(FaceType.Down, 0);
        var leftRight = GetColumn(FaceType.Left, 2);

        if (direction == RotationDirection.Clockwise)
        {
            SetColumn(FaceType.Right, 0, upBottom);
            SetRow(FaceType.Down, 0, Reverse(rightLeft));
            SetColumn(FaceType.Left, 2, downTop);
            SetRow(FaceType.Up, 2, Reverse(leftRight));
        }
        else
        {
            SetColumn(FaceType.Left, 2, Reverse(upBottom));
            SetRow(FaceType.Down, 0, leftRight);
            SetColumn(FaceType.Right, 0, Reverse(downTop));
            SetRow(FaceType.Up, 2, rightLeft);
        }
    }

    private void RotateBack(RotationDirection direction)
    {
        RotateFaceStickers(FaceType.Back, direction);

        var upTop = GetRow(FaceType.Up, 0);
        var leftLeft = GetColumn(FaceType.Left, 0);
        var downBottom = GetRow(FaceType.Down, 2);
        var rightRight = GetColumn(FaceType.Right, 2);

        if (direction == RotationDirection.Clockwise)
        {
            SetColumn(FaceType.Left, 0, Reverse(upTop));
            SetRow(FaceType.Down, 2, leftLeft);
            SetColumn(FaceType.Right, 2, Reverse(downBottom));
            SetRow(FaceType.Up, 0, rightRight);
        }
        else
        {
            SetColumn(FaceType.Right, 2, upTop);
            SetRow(FaceType.Down, 2, Reverse(rightRight));
            SetColumn(FaceType.Left, 0, downBottom);
            SetRow(FaceType.Up, 0, Reverse(leftLeft));
        }
    }

    private void RotateLeft(RotationDirection direction)
    {
        RotateFaceStickers(FaceType.Left, direction);

        var frontLeft = GetColumn(FaceType.Front, 0);
        var upLeft = GetColumn(FaceType.Up, 0);
        var backRight = GetColumn(FaceType.Back, 2);
        var downLeft = GetColumn(FaceType.Down, 0);

        if (direction == RotationDirection.Clockwise)
        {
            SetColumn(FaceType.Down, 0, frontLeft);
            SetColumn(FaceType.Back, 2, Reverse(downLeft));
            SetColumn(FaceType.Up, 0, Reverse(backRight));
            SetColumn(FaceType.Front, 0, upLeft);
        }
        else
        {
            SetColumn(FaceType.Up, 0, frontLeft);
            SetColumn(FaceType.Back, 2, Reverse(upLeft));
            SetColumn(FaceType.Down, 0, Reverse(backRight));
            SetColumn(FaceType.Front, 0, downLeft);
        }
    }

    private void RotateRight(RotationDirection direction)
    {
        RotateFaceStickers(FaceType.Right, direction);

        var frontRight = GetColumn(FaceType.Front, 2);
        var downRight = GetColumn(FaceType.Down, 2);
        var backLeft = GetColumn(FaceType.Back, 0);
        var upRight = GetColumn(FaceType.Up, 2);

        if (direction == RotationDirection.Clockwise)
        {
            SetColumn(FaceType.Up, 2, frontRight);
            SetColumn(FaceType.Back, 0, Reverse(upRight));
            SetColumn(FaceType.Down, 2, Reverse(backLeft));
            SetColumn(FaceType.Front, 2, downRight);
        }
        else
        {
            SetColumn(FaceType.Down, 2, frontRight);
            SetColumn(FaceType.Back, 0, Reverse(downRight));
            SetColumn(FaceType.Up, 2, Reverse(backLeft));
            SetColumn(FaceType.Front, 2, upRight);
        }
    }

    private void RotateUp(RotationDirection direction)
    {
        RotateFaceStickers(FaceType.Up, direction);

        var frontTop = GetRow(FaceType.Front, 0);
        var leftTop = GetRow(FaceType.Left, 0);
        var backTop = GetRow(FaceType.Back, 0);
        var rightTop = GetRow(FaceType.Right, 0);

        if (direction == RotationDirection.Clockwise)
        {
            SetRow(FaceType.Left, 0, frontTop);
            SetRow(FaceType.Back, 0, leftTop);
            SetRow(FaceType.Right, 0, backTop);
            SetRow(FaceType.Front, 0, rightTop);
        }
        else
        {
            SetRow(FaceType.Right, 0, frontTop);
            SetRow(FaceType.Back, 0, rightTop);
            SetRow(FaceType.Left, 0, backTop);
            SetRow(FaceType.Front, 0, leftTop);
        }
    }

    private void RotateDown(RotationDirection direction)
    {
        RotateFaceStickers(FaceType.Down, direction);

        var frontBottom = GetRow(FaceType.Front, 2);
        var rightBottom = GetRow(FaceType.Right, 2);
        var backBottom = GetRow(FaceType.Back, 2);
        var leftBottom = GetRow(FaceType.Left, 2);

        if (direction == RotationDirection.Clockwise)
        {
            SetRow(FaceType.Right, 2, frontBottom);
            SetRow(FaceType.Back, 2, rightBottom);
            SetRow(FaceType.Left, 2, backBottom);
            SetRow(FaceType.Front, 2, leftBottom);
        }
        else
        {
            SetRow(FaceType.Left, 2, frontBottom);
            SetRow(FaceType.Back, 2, leftBottom);
            SetRow(FaceType.Right, 2, backBottom);
            SetRow(FaceType.Front, 2, rightBottom);
        }
    }

    #endregion

    /// <summary>
    /// Rotates the stickers on a single face
    /// </summary>
    private void RotateFaceStickers(FaceType face, RotationDirection direction)
    {
        var temp = new ColorType[3, 3];
        int faceIndex = (int)face;

        if (direction == RotationDirection.Clockwise)
        {
            for (int r = 0; r < 3; r++)
                for (int c = 0; c < 3; c++)
                    temp[c, 2 - r] = _faces[faceIndex, r, c];
        }
        else
        {
            for (int r = 0; r < 3; r++)
                for (int c = 0; c < 3; c++)
                    temp[2 - c, r] = _faces[faceIndex, r, c];
        }

        for (int r = 0; r < 3; r++)
            for (int c = 0; c < 3; c++)
                _faces[faceIndex, r, c] = temp[r, c];
    }

    private ColorType[] GetRow(FaceType face, int row)
    {
        int faceIndex = (int)face;
        return [_faces[faceIndex, row, 0], _faces[faceIndex, row, 1], _faces[faceIndex, row, 2]];
    }

    private void SetRow(FaceType face, int row, ColorType[] colors)
    {
        int faceIndex = (int)face;
        for (int c = 0; c < 3; c++)
            _faces[faceIndex, row, c] = colors[c];
    }

    private ColorType[] GetColumn(FaceType face, int col)
    {
        int faceIndex = (int)face;
        return [_faces[faceIndex, 0, col], _faces[faceIndex, 1, col], _faces[faceIndex, 2, col]];
    }

    private void SetColumn(FaceType face, int col, ColorType[] colors)
    {
        int faceIndex = (int)face;
        for (int r = 0; r < 3; r++)
            _faces[faceIndex, r, col] = colors[r];
    }

    private static ColorType[] Reverse(ColorType[] array) => [array[2], array[1], array[0]];
}

