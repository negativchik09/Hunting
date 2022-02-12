using Hunting.BL.Enum;
using Hunting.BL.Matrix;

namespace Hunting.BL.Abstractions;

public abstract class Unit
{
    public string UnitType { get; init; }
    public double Hp { get; }
    public string Name { get; }
    public Node Node { get; set; }
    public double Hunger { get; } 
    public Direction Direction { get; }

    public abstract UnitCommandExecutionResult Step();
    public abstract UnitCommandExecutionResult TurnLeft();
    public abstract UnitCommandExecutionResult TurnRight();
    public abstract UnitCommandExecutionResult Eat();
}