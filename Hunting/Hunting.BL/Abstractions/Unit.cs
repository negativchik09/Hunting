using Hunting.BL.Enum;
using Hunting.BL.Matrix;
using Newtonsoft.Json;

namespace Hunting.BL.Abstractions;

public abstract class Unit
{
    private double _hunger;
    private double _hp;
    public static HashSet<string> UnitNames { get; }
    public string UnitType { get; init; }

    public double Hp
    {
        get
        {
            return _hp;
        }
        set
        {
            if (value <= 0)
            {
                Die();
            }

            _hp = value;
        }
    }

    public string Name { get; }

    public double Hunger
    {
        get
        {
            return _hunger;
        }
        set
        {
            if (value < 0)
            {
                Hp += value;
                _hunger = 0;
                return;
            }

            _hunger = value;
        }
    }

    public int VisibilityRange { get; init; }
    public double VisibilityAngle { get; init; }
    public Direction Direction { get; private set; }
    [JsonIgnore]
    public Node Node { get; internal set; }
    [JsonIgnore]
    public int X => Node.X;

    [JsonIgnore]
    public int Y => Node.Y;
    
    protected Unit(double hp, string name, double hunger, Node node, string unitType)
    {
        Hp = hp;
        Name = name;
        Hunger = hunger;
        Direction = Direction.Bot;
        Node = node;
        UnitType = unitType;

        UnitNames.Add(name);
    }

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

    public virtual UnitCommandExecutionResult Die()
    {
        Node.Unit = null;
        return UnitNames.Remove(Name) 
            ? UnitCommandExecutionResult.Executed 
            : UnitCommandExecutionResult.UnableExecute;
    }
    
    public abstract UnitCommandExecutionResult Eat();
}