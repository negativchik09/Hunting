using Hunting.BL.Abstractions;
using Hunting.BL.Enum;
using Hunting.BL.Matrix;

namespace Hunting.BL.Units;

public class Wolf : Unit
{ 
    internal Wolf(string name, Node? node) : base(100, name, 100, node, nameof(Wolf))
    { }

    public override void Eat()
    {
        throw new NotImplementedException();
    }

    public override bool CanEat()
    {
        if (NodeAggregator.NeighbouringNodes(Node, NeighbourType.Diagonal).Any(x => x.Meat.Length))
    }
}