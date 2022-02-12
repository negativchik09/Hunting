using Hunting.BL.Abstractions;
using Hunting.BL.Enum;
using Newtonsoft.Json;

namespace Hunting.BL.Matrix;

public class Node
{
    public Unit? Unit { get; internal set; }
    public Surface Surface { get; internal set; }
    public Meat[] Meat { get; internal set; }
    public int X { get; internal set; }
    public int Y { get; internal set; }
    [JsonIgnore]
    internal int TurnsAfterGrassEating { get; set; }
    internal Node(){}
}