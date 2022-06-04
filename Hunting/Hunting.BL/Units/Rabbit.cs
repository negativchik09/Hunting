using Hunting.BL.Abstractions;
using Hunting.BL.Enum;
using Hunting.BL.Matrix;

namespace Hunting.BL.Units;

public class Rabbit : Unit
{
    internal Rabbit(string name, Node? node) : base(20, name, 20, node, nameof(Rabbit))
    { }

    public override void Eat()
    {
        Hunger += 10;
        Node.Surface = Surface.Ground;
    }

    public override bool CanEat()
    {
        return Node.Surface == Surface.Grass;
    }

    public override void Die()
    {
        Node.Meat
        base.Die();
    }
}