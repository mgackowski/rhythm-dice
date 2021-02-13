using System.Collections.Generic;

public class DieSide
{
    public int Value { get; }
    public Dictionary<Direction, DieSide> Sides;
    public Dictionary<Direction, Direction> Orientations;

    public DieSide(int value, Dictionary<Direction, DieSide> sides, Dictionary<Direction, Direction> orientations)
    {
        this.Value = value;
        this.Sides = sides;
        this.Orientations = orientations;
    }

}
