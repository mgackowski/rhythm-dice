using UnityEngine;

public enum Direction
{
    Up,
    Right,
    Down,
    Left,
    None
}

static class DirectionExtensions
{
    public static Direction ReverseDirection(this Direction dir)
    {
        switch (dir)
        {
            case Direction.Up: return Direction.Down;
            case Direction.Right: return Direction.Left;
            case Direction.Down: return Direction.Up;
            case Direction.Left: return Direction.Right;
            case Direction.None: return Direction.None;
            default: return Direction.Up;
        }
    }

    public static Vector3 DirectionToVector3(this Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Vector3.up;
            case Direction.Right:
                return Vector3.right;
            case Direction.Down:
                return Vector3.down;
            case Direction.Left:
                return Vector3.left;
            default: return Vector3.zero;
        }
    }
}