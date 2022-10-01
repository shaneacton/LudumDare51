using System;
using UnityEngine;

public class Node : IComparable<Node>
{
    public Vector2 Position;
    public float Priority;

    public int x => (int) Math.Round(Position.x);
    public int y => (int) Math.Round(Position.y);

    public Node(int x, int y)
    {
        Position = new Vector2(x, y);
    }

    public int CompareTo(Node other)
    {
        if (this.Priority < other.Priority) return -1;
        else if (this.Priority > other.Priority) return 1;
        else return 0;
    }

    public override bool Equals(object obj)
    {
        if (obj is Node otherNode)
        {
            return otherNode.x == x && otherNode.y == y;
        }
        throw new Exception("");
    }

    public override int GetHashCode()
    {
        return x * 13 + y * 547;
    }

    public override string ToString()
    {
        return "Node: " + x + ", " + y;
    }
}
