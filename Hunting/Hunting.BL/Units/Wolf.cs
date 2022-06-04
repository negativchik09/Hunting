using Hunting.BL.Abstractions;
using Hunting.BL.Enum;
using Hunting.BL.Matrix;

namespace Hunting.BL.Units;

public class Wolf : Unit
{ 
    internal Wolf(string name, Node? node) : base(50, name, 40, node, nameof(Wolf))
    { }

    public override void Eat()
    {
        var nodes = NodeAggregator.NeighbouringNodes(Node, NeighbourType.Diagonal)
            .Where(x => x.Meat.Count > 0);
        foreach (var node in nodes)
        {
            Hunger += node.Meat.First().HungerRegen;
            if (Hunger == 40)
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
            Amount = 2,
            HungerRegen = 17,
            TurnsBeforeDispose = 4
        });
        base.Die();
    }
}