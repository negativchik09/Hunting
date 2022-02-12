using Hunting.BL.Abstractions;
using Hunting.BL.Enum;
using Hunting.BL.Matrix;

namespace Hunting.BL.Units;

public class Rabbit : IUnit
{
    public string UnitType { get; init; }
    public double Hp { get; }
    public string Name { get; }
    public Node Node { get; set; }
    public double Hunger { get; }
    public Direction Direction { get; }

    internal Rabbit()
    {
        UnitType = nameof(Rabbit);
    }
}