using Hunting.BL.Abstractions;
using Hunting.BL.Enum;
using Hunting.BL.Special;
using Hunting.BL.Units;

namespace Hunting.BL.Matrix;

internal class Pathfinder
{
    public Node[] Fow(IUnit unit)
    {
        NodeAggregator.GetNeighbouringNodes(unit.Node, NeighbourType.Diagonal);
        throw new NotImplementedException();
    }

    public MovingPathFindResult IsCanMove(IUnit unit, Node node)
    {
        throw new NotImplementedException();
    }
}