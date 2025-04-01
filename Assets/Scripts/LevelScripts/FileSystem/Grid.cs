using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grid : DropArea
{
    public GridSystem GridSystem { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    public Grid GetNeighbour(Direction dir)
    {
        int x = X;
        int y = Y;
        switch (dir)
        {
            case Direction.Up:
                y++;
                break;
            case Direction.Down:
                y--;
                break;
            case Direction.Left:
                x--;
                break;
            case Direction.Right:
                x++;
                break;
        }

        if (x < 0 || y < 0 || x >= GridSystem.HSize || y >= GridSystem.VSize)
        {
            return null;
        }

        return GridSystem.GetGrid(x, y);
    }

    public override string ToString()
    {
        return "(" + X + "," + Y + ")";
    }

    public enum Direction
{
    Up,
    Down,
    Left,
    Right

}

}
