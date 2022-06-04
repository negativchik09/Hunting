using Hunting.BL.Abstractions;
using Hunting.BL.Enum;
using Newtonsoft.Json;

namespace Hunting.BL.Matrix;

public class Node : IEquatable<Node>
{
    public Unit? Unit { get; set; }
    public Surface Surface { get; set; }
    public Meat[] Meat { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    [JsonIgnore]
    internal int TurnsAfterGrassEating { get; set; }

    public bool Equals(Node? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Node)obj);
    }

    public override int GetHashCode()
    {
        return (X * 397) ^ Y;
    }
}