using Hunting.BL.Abstractions;
using Hunting.BL.Enum;
using Hunting.BL.Matrix;

namespace Hunting.BL.Units;

public class Huntsman : Unit
{
    internal Huntsman(string name, Node? node) : base(100, name, 100, node, nameof(Huntsman))
    { }

    public override void Eat()
    {
        var nodes = NodeAggregator.NeighbouringNodes(Node, NeighbourType.Diagonal)
            .Where(x => x.Meat.Count > 0);
        foreach (var node in nodes)
        {
            Hunger += node.Meat.First().HungerRegen;
            if (Hunger == 100)
            {
                return;
            }
        }
    }

    public override bool CanEat()
    {
        return NodeAggregator.NeighbouringNodes(Node, NeighbourType.Diagonal)
            .Any(x => x.Meat.Any());
    }
    
    public override void Die()
    {
        Node.Meat.Add(new Meat()
        {
            Amount = 4,
            HungerRegen = 20,
            TurnsBeforeDispose = 3
        });
        base.Die();
    }
}