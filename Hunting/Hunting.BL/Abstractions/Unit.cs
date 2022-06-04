using Hunting.BL.Enum;
using Hunting.BL.Matrix;
using Hunting.BL.Units;
using Newtonsoft.Json;

namespace Hunting.BL.Abstractions;

public abstract class Unit
{
    private int _hunger;
    private double _hp;
    public static IEnumerable<Unit> Units => UnitIsHasCommandDict.Keys;
    public static Dictionary<Unit, bool> UnitIsHasCommandDict { get; } = new();
    public string UnitType { get; init; }

    public int NodesPerTurn => UnitType switch
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

    public int VisibilityRange { get; init; }
    public double VisibilityAngle { get; init; }
    public Direction Direction { get; private set; }
    [JsonIgnore] public Node Node { get; internal set; }
    [JsonIgnore] public int X => Node.X;

    [JsonIgnore] public int Y => Node.Y;

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
        if (node.Surface is Surface.Tree or Surface.Water || node.Unit == null) return;
        Node.Unit = null;
        node.Unit = this;
        Node = node;
    }

    public virtual void Die()
    {
        Node.Unit = null;
        UnitIsHasCommandDict.Remove(this);
    }
    
    public abstract void Eat();

    public abstract bool CanEat();

    public abstract ICommand? GetNextCommand();
}