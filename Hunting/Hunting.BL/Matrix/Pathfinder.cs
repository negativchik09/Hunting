using Hunting.BL.Abstractions;
using Hunting.BL.Enum;
using Hunting.BL.Special;

namespace Hunting.BL.Matrix;

internal class Pathfinder
{
    public Node[] Fow(Unit unit)
    {
        IEnumerable<Node> neighbouringNodes = NodeAggregator.GetNeighbouringNodes(unit.Node, NeighbourType.Diagonal);
        throw new NotImplementedException();
    }

    public MovingPathFindResult FindPath(Unit unit, Node node)
    {
        throw new NotImplementedException();
    }
}