using Hunting.BL.Abstractions;
using Hunting.BL.Enum;
using Newtonsoft.Json;

namespace Hunting.BL.Matrix;

public class Node
{
    public Unit? Unit { get; set; }
    public Surface Surface { get; set; }
    public Meat[] Meat { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    [JsonIgnore]
    internal int TurnsAfterGrassEating { get; set; }
}