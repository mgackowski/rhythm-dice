using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieMovementModel
{
    public struct DieMoveResult
    {
        public DieSide NewSide;
        public Direction NewOrientation;
        public Vector3 RotationPivot;
    }

    private static int Mod(int a, int b)
    {
        return (a % b + b) % b;
    }


    public static DieMoveResult Move(DieSide currentSide, Direction currentOrientation, Direction direction)
    {
        DieSide newSide;
        Direction newOrientation;
        Direction newOrientationGlobal;
        Vector3 rotationPivot;

        switch(direction)
        {
            case Direction.Up:
                direction = Direction.Down;
                break;
            case Direction.Down:
                direction = Direction.Up;
                break;
            case Direction.Left:
                direction = Direction.Right;
                break;
            case Direction.Right:
                direction = Direction.Left;
                break;

        }

        newSide = currentSide.Sides[(Direction)Mod(((int)direction - (int)currentOrientation), 4)];
        newOrientation = currentSide.Orientations[(Direction)Mod(((int)direction - (int)currentOrientation), 4)];
        newOrientationGlobal = (Direction)Mod((int)newOrientation + (int)currentOrientation,4);
        rotationPivot = Vector3.zero; //Unused; global coordinates used in Die class

        DieMoveResult result = new DieMoveResult
        {
            NewSide = newSide,
            NewOrientation = newOrientationGlobal,
            RotationPivot = rotationPivot
        };
        return result;
    }


}
