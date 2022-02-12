using Hunting.BL.Enum;
using Hunting.BL.Matrix;

namespace Hunting.BL.Abstractions;

public interface IUnit
{
    string UnitType { get; init; }
    double Hp { get; }
    string Name { get; }
    Node Node { get; set; }
    double Hunger { get; } 
    Direction Direction { get; }
}