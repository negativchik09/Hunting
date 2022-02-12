using Hunting.BL.Abstractions;
using Hunting.BL.Enum;
using Newtonsoft.Json;

namespace Hunting.BL.Matrix;

public class Node
{
    public IUnit? Unit { get; internal set; }
    public Surface Surface { get; internal set; }
    public Meat[] Meat { get; internal set; }
    internal int X { get; init; }
    internal int Y { get; init; }
    [JsonIgnore]
    internal int TurnsAfterGrassEating { get; set; }
    internal Node(){}
}