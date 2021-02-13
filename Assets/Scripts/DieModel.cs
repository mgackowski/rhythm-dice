using System.Collections.Generic;

public class DieModel
{
    public Dictionary<Side, DieSide> Sides { get; }

    public DieModel()
    {
        // Initialise a standard clockwise 6d
        DieSide top = new DieSide(1, null, null);
        DieSide back = new DieSide(2, null, null);
        DieSide bottom = new DieSide(6, null, null);
        DieSide front = new DieSide(5, null, null);
        DieSide left = new DieSide(4, null, null);
        DieSide right = new DieSide(3, null, null);

        top.Sides = new Dictionary<Direction, DieSide>()
        {
            [Direction.Up] = back,
            [Direction.Right] = right,
            [Direction.Down] = front,
            [Direction.Left] = left
        };
        top.Orientations = new Dictionary<Direction, Direction>()
        {
            [Direction.Up] = Direction.Up,
            [Direction.Right] = Direction.Right,
            [Direction.Down] = Direction.Up,
            [Direction.Left] = Direction.Right
        };

        back.Sides = new Dictionary<Direction, DieSide>()
        {
            [Direction.Up] = bottom,
            [Direction.Right] = right,
            [Direction.Down] = top,
            [Direction.Left] = left
        };
        back.Orientations = new Dictionary<Direction, Direction>()
        {
            [Direction.Up] = Direction.Up,
            [Direction.Right] = Direction.Up,
            [Direction.Down] = Direction.Up,
            [Direction.Left] = Direction.Down
        };

        bottom.Sides = new Dictionary<Direction, DieSide>()
        {
            [Direction.Up] = front,
            [Direction.Right] = right,
            [Direction.Down] = back,
            [Direction.Left] = left
        };
        bottom.Orientations = new Dictionary<Direction, Direction>()
        {
            [Direction.Up] = Direction.Up,
            [Direction.Right] = Direction.Left,
            [Direction.Down] = Direction.Up,
            [Direction.Left] = Direction.Left
        };

        front.Sides = new Dictionary<Direction, DieSide>()
        {
            [Direction.Up] = top,
            [Direction.Right] = right,
            [Direction.Down] = bottom,
            [Direction.Left] = left
        };
        front.Orientations = new Dictionary<Direction, Direction>()
        {
            [Direction.Up] = Direction.Up,
            [Direction.Right] = Direction.Down,
            [Direction.Down] = Direction.Up,
            [Direction.Left] = Direction.Up
        };

        left.Sides = new Dictionary<Direction, DieSide>()
        {
            [Direction.Up] = top,
            [Direction.Right] = front,
            [Direction.Down] = bottom,
            [Direction.Left] = back
        };
        left.Orientations = new Dictionary<Direction, Direction>()
        {
            [Direction.Up] = Direction.Left,
            [Direction.Right] = Direction.Up,
            [Direction.Down] = Direction.Right,
            [Direction.Left] = Direction.Down
        };

        right.Sides = new Dictionary<Direction, DieSide>()
        {
            [Direction.Up] = bottom,
            [Direction.Right] = front,
            [Direction.Down] = top,
            [Direction.Left] = back
        };
        right.Orientations = new Dictionary<Direction, Direction>()
        {
            [Direction.Up] = Direction.Right,
            [Direction.Right] = Direction.Down,
            [Direction.Down] = Direction.Left,
            [Direction.Left] = Direction.Up
        };

        Sides = new Dictionary<Side, DieSide>()
        {
            [Side.Top] = top,
            [Side.Left] = left,
            [Side.Front] = front,
            [Side.Right] = right,
            [Side.Back] = back,
            [Side.Bottom] = bottom
        };

    }


}
