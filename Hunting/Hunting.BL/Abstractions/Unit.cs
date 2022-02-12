using Hunting.BL.Enum;
using Hunting.BL.Matrix;
using Newtonsoft.Json;

namespace Hunting.BL.Abstractions;

public abstract class Unit
{
    protected Unit(double hp, string name, double hunger, Node node)
    {
        Hp = hp;
        Name = name;
        Hunger = hunger;
        Direction = Direction.Bot;
        Node = node;
    }

    public string UnitType { get; init; }
    public double Hp { get; }
    public string Name { get; }
    [JsonIgnore]
    public Node Node { get; set; }
    public double Hunger { get; } 
    public Direction Direction { get; }

    public abstract UnitCommandExecutionResult Step();
    public abstract UnitCommandExecutionResult TurnLeft();
    public abstract UnitCommandExecutionResult TurnRight();
    public abstract UnitCommandExecutionResult Eat();
}