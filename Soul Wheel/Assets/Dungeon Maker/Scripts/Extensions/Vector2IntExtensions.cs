using System;
using Vector2Int = UnityEngine.Vector2Int;

public static class Vector2IntExtensions
{
    public static Vector2Int RandomDirection(this Vector2Int v)
    {
        Random rand = new Random();

        switch (rand.Next(2))
        {
            case 0: v = v.RandomLinealDirection(); break;
            case 1: v = v.RandomDiagonalDirection(); break;
        }

        return v;
    }

    public static Vector2Int RandomLinealDirection(this Vector2Int v)
    {
        Random rand = new Random();

        switch (rand.Next(4))
        {
            case 0: v = Vector2Int.up; break;
            case 1: v = Vector2Int.down; break;
            case 2: v = Vector2Int.left; break;
            case 3: v = Vector2Int.right; break;
        }

        return v;
    }

    public static Vector2Int RandomDiagonalDirection(this Vector2Int v)
    {
        Random rand = new Random();

        switch (rand.Next(4))
        {
            case 0: v = new Vector2Int(-1, 1); break;
            case 1: v = new Vector2Int(1, 1); break;
            case 2: v = new Vector2Int(-1, -1); break;
            case 3: v = new Vector2Int(1, -1); break;
        }

        return v;
    }
}