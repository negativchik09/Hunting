using Hunting.BL.Commands.Contracts;
using Hunting.BL.Commands.UnitCommands;
using Hunting.BL.Commands.User;
using Hunting.BL.Enum;
using Hunting.BL.Matrix;
using Hunting.BL.Special;
using Hunting.BL.Units;
using Newtonsoft.Json;

namespace Hunting.BL.Abstractions;

public abstract class Unit : IEquatable<Unit>
{
    private int _hunger;
    private double _hp;
    [JsonIgnore] public static IEnumerable<Unit> Units => UnitIsHasCommandDict.Keys;
    [JsonIgnore] public static Dictionary<Unit, bool> UnitIsHasCommandDict { get; } = new();
    public string UnitType { get; init; }

    [JsonIgnore] public int NodesPerTurn => UnitType switch
    {
        nameof(Wolf) => 3,
        nameof(Rabbit) => 2,
        nameof(Huntsman) => 2,
        _ => throw new ArgumentOutOfRangeException()
    };

    public double Hp
    {
        get { return _hp; }
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

    public int Hunger
    {
        get { return _hunger; }
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

    [JsonIgnore] public int VisibilityRange { get; init; }
    [JsonIgnore] public double VisibilityAngle { get; init; }
    [JsonIgnore] public Direction Direction { get; private set; }
    [JsonIgnore] public Node Node { get; internal set; }
    public int X => Node.X;

    public int Y => Node.Y;

    protected Unit(double hp, string name, int hunger, Node node, string unitType, double visibilityAngle, int visibilityRange)
    {
        Hp = hp;
        Name = name;
        Hunger = hunger;
        Direction = Direction.Bot;
        Node = node;
        UnitType = unitType;
        VisibilityAngle = visibilityAngle;
        VisibilityRange = visibilityRange;

        UnitIsHasCommandDict.Add(this, false);
    }

    public virtual void Step(Node node)
    {
        if (!NodeAggregator.CanStepOnNode(node)) return;
        Node.Unit = null;
        node.Unit = this;
        Node = node;
        Hunger -= 2;
    }

    public virtual void Die()
    {
        CommandExecutor.Instance.AddCommand(new RemoveUnitCommand($"{UnitType} {Name} died at {Node.X}:{Node.Y}")
        {
            Contract = new RemoveUnitContract(X, Y),
            State = UserCommandExecutionResult.Valid
        });
    }

    public abstract void Eat();

    public abstract bool CanEat();

    public abstract ICommand GetNextCommand();

    public bool Equals(Unit? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return UnitType == other.UnitType && Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Unit)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (UnitType.GetHashCode() * 397) ^ Name.GetHashCode();
        }
    }
    
    protected ICommand StepOnRandomInFow(IEnumerable<Node> fowPoints)
    {
        var filtered = fowPoints.Where(NodeAggregator.CanStepOnNode)
            .ToList();

        if (!filtered.Any())
        {
            return new RestUnitCommand(
                $"{UnitType} {Name} hasn`t found grass or enemies in it's FOW at {Node.X}:{Node.Y} and decided to rest")
            {
                Contract = new RestUnitContract(this)
            };
        }
        
        var randIndex = new Random().Next(0, filtered.Count);
        
        var randPoint = Pathfinder.FindNearestNode(Node, filtered[randIndex], NodesPerTurn);

        return new MoveUnitCommand(
            $"{UnitType} {Name} hasn`t found grass or enemies in it's FOW at {Node.X}:{Node.Y} and moved to {randPoint.X}:{randPoint.Y}")
        {
            Contract = new MoveUnitContract(this, randPoint)
        };
    }
}