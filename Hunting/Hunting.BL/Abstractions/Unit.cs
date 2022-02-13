using Hunting.BL.Enum;
using Hunting.BL.Matrix;
using Newtonsoft.Json;

namespace Hunting.BL.Abstractions;

public abstract class Unit
{
    protected Unit(double hp, string name, double hunger, Node node, string unitType)
    {
        Hp = hp;
        Name = name;
        Hunger = hunger;
        Direction = Direction.Bot;
        Node = node;
        UnitType = unitType;
    }

    public string UnitType { get; init; }
    public double Hp { get; internal set; }
    public string Name { get; }
    public double Hunger { get; internal set; }
    public int VisibilityRange { get; init; }
    public double VisibilityAngle { get; init; }
    public Direction Direction { get; private set; }
    [JsonIgnore]
    public Node Node { get; private set; }
    [JsonIgnore]
    public int X => Node.X;

    [JsonIgnore]
    public int Y => Node.Y;

    public virtual UnitCommandExecutionResult Step()
    {
        var node = NodeAggregator.NeighbourNode(Node, Direction);
        if (node == null)
        {
            return UnitCommandExecutionResult.UnableExecute;
        }

        Node = node;
        return UnitCommandExecutionResult.Executed;
    }

    public virtual UnitCommandExecutionResult TurnLeft()
    {
        Direction = Direction.LeftSide();
        return UnitCommandExecutionResult.Executed;
    }

    public virtual UnitCommandExecutionResult TurnRight()
    {
        Direction = Direction.RightSide();
        return UnitCommandExecutionResult.Executed;
    }
    public abstract UnitCommandExecutionResult Eat();
}