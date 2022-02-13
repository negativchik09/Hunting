using Hunting.BL.Abstractions;
using Hunting.BL.Enum;
using Hunting.BL.Special;

namespace Hunting.BL.Matrix;

internal static class Pathfinder
{
    public static IEnumerable<Node> Fow(Unit unit)
    {
        IEnumerable<Node> nodes = NodeAggregator.Segment(
            unit.Node,
            unit.Direction,
            unit.VisibilityRange,
            unit.VisibilityAngle);

        var start = unit.Node;
        foreach (var node in nodes)
        {
            int xRange = node.X - start.X;
            int yRange = node.Y - start.Y;
            var checkQueue = new Queue<Node>();
            Direction? nextStepDirection;
            Node current;
            Node? neighbour;
            checkQueue.Enqueue(start);
            while (checkQueue.Count > 0)
            {
                current = checkQueue.Dequeue();

                if (current == node)
                {
                    yield return node;
                }
                
                if (current.Surface == Surface.Tree)
                {
                    break;
                }

                nextStepDirection = DirectionByRanges(xRange, yRange);
                if (nextStepDirection != null)
                {
                    neighbour = NodeAggregator.NeighbourNode(current, (Direction)nextStepDirection);
                    EnqueueIfNotNull(neighbour, checkQueue);
                    continue;
                }
                
                neighbour = xRange > 0
                    ? NodeAggregator.NeighbourNode(current, Direction.Right)
                    : NodeAggregator.NeighbourNode(current, Direction.Left);
                EnqueueIfNotNull(neighbour, checkQueue);
                neighbour = yRange > 0
                    ? NodeAggregator.NeighbourNode(current, Direction.Bot)
                    : NodeAggregator.NeighbourNode(current, Direction.Top);
                EnqueueIfNotNull(neighbour, checkQueue);
            }
        }
    }

    private static Direction? DirectionByRanges(int xRange, int yRange)
    {
        if (Math.Abs(xRange) > Math.Abs(yRange))
        {
            return xRange > 0 ? Direction.Right : Direction.Left;
        }

        if (Math.Abs(xRange) < Math.Abs(yRange))
        {
            return yRange > 0 ? Direction.Bot : Direction.Top;
        }

        return null;
    }
    
    private static void EnqueueIfNotNull(Node? node, Queue<Node> queue)
    {
        if (node != null)
        {
            queue.Enqueue(node);
        }
    }

    public static MovingPathFindResult FindPath(Unit unit, Node node)
    {
        return null;
    }
}