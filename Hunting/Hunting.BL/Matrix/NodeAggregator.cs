using System.Collections.ObjectModel;
using Hunting.BL.Enum;

namespace Hunting.BL.Matrix;

internal static class NodeAggregator
{
    public static readonly int MatrixSize = 40;
    private static Node[,] _nodes;
    internal static IEnumerable<Node> NodeList
    {
        get
        {
            for (int i = 0; i < MatrixSize; i++)
            {
                for (int j = 0; j < MatrixSize; j++)
                {
                    yield return _nodes[i, j];
                }
            }
        }
        set
        {
            foreach (var node in value)
            {
                _nodes[node.X, node.Y] = node;
            }
        }
    }

    public static ReadOnlyCollection<Node> Nodes
    {
        get => new ReadOnlyCollection<Node>(new List<Node>(NodeList));
        internal set => NodeList = value;
    }

    static NodeAggregator()
    {
        _nodes = new Node[MatrixSize, MatrixSize];
    }

    public static IEnumerable<Node> NeighbouringNodes(Node node, NeighbourType type)
    {
        bool isNotOnLeftBorder = node.X - 1 >= 0;
        bool isNotOnRightBorder = node.X + 1 < MatrixSize;
        bool isNotOnTopBorder = node.Y - 1 >= 0;
        bool isNotOnBotBorder = node.X - 1 < MatrixSize;

        if (isNotOnLeftBorder)
        {
            yield return _nodes[node.X - 1, node.Y];
        }
        if (isNotOnRightBorder)
        {
            yield return _nodes[node.X + 1, node.Y];
        }
        if (isNotOnTopBorder)
        {
            yield return _nodes[node.X, node.Y - 1];
        }
        if (isNotOnBotBorder)
        {
            yield return _nodes[node.X, node.Y + 1];
        }

        if (type == NeighbourType.Side) yield break;
        
        if (isNotOnTopBorder && isNotOnLeftBorder)
        {
            yield return _nodes[node.X - 1, node.Y - 1];
        }
        if (isNotOnBotBorder && isNotOnLeftBorder)
        {
            yield return _nodes[node.X - 1, node.Y + 1];
        }
        if (isNotOnTopBorder && isNotOnRightBorder)
        {
            yield return _nodes[node.X + 1, node.Y - 1];
        }
        if (isNotOnBotBorder && isNotOnRightBorder)
        {
            yield return _nodes[node.X + 1, node.Y + 1];
        }
    }

    public static bool IsNeighbouring(Node node1, Node node2, NeighbourType type)
    {
        int rangeByX = Math.Abs(node1.X - node2.X);
        int rangeByY = Math.Abs(node1.Y - node2.Y);
        
        if (rangeByX >= 2 || rangeByY >= 2) return false;
        
        return type == NeighbourType.Diagonal || rangeByX + rangeByY < 2;
    }

    public static Node? NeighbourNode(Node node, Direction direction)
    {
        return direction switch
        {
            Direction.Left => node.X - 1 >= 0 ? _nodes[node.X - 1, node.Y] : null,
            Direction.Top => node.Y - 1 >= 0 ? _nodes[node.X, node.Y - 1] : null,
            Direction.Right => node.X + 1 < MatrixSize ? _nodes[node.X + 1, node.Y] : null,
            Direction.Bot => node.Y + 1 < MatrixSize ? _nodes[node.X, node.Y + 1] : null,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    public static IEnumerable<Node> VectorFromNode(Node? node, Direction direction)
    {
        var getNext = new Func<Node?, Node?>((prevNode) => NeighbourNode(prevNode, direction));
        
        var current = node;
        for (int i = 0;; i++)
        {
            current = getNext(current);
            if (current == null)
            {
                yield break;
            }
            yield return current;
        }
    }

    public static IEnumerable<Node> Segment(Node? node, int range)
    {
        List<Node> nodes = new List<Node>() { node };

        for (int i = 0; i < range; i++)
        {
            var buffer = new List<Node>();
            foreach (var node_ in nodes)
            {
                buffer.AddRange(NeighbouringNodes(node_, NeighbourType.Diagonal)
                    .Except(nodes)
                    .Except(buffer));
            }
            nodes.AddRange(buffer);
        }

        nodes.Remove(node);

        /*
        double phi = segmentRadians / 2;

        int height = Math.Abs((int)Math.Round(range * Math.Sin(phi)));
        int vectorSize;
        double tanPhi = Math.Tan(phi);
        int diff = range - nodes.Count;
        int pivot = diff - 1;
        Node? leftSide = nodes.Last();
        Node? rightSide = nodes.Last();

        for (int i = 0; height > 0; height--, i++)
        {
            vectorSize = (int)Math.Ceiling(height / tanPhi);
            if (vectorSize + 2 * pivot == range)
            {
                pivot++;
            }
            
            vectorSize = vectorSize - diff + 1;
            
            if (leftSide != null)
            {
                leftSide = NeighbourNode(leftSide, direction.LeftSide());
                nodes.AddRange(
                    VectorFromNode(leftSide, direction.ReverseSide())
                    .Skip(pivot)
                    .Take(vectorSize));
            }
            
            if (rightSide != null)
            {
                rightSide = NeighbourNode(rightSide, direction.RightSide());
                nodes.AddRange(
                    VectorFromNode(rightSide, direction.ReverseSide())
                        .Skip(pivot)
                        .Take(vectorSize));
            }
        }*/

        return nodes;
    }

    public static Node GetNode(int x, int y)
    {
        return _nodes[x, y];
    }

    public static bool CanStepOnNode(Node x)
    {
        if (x.Surface == Surface.Tree)
        {
            return false;
        }

        if (x.Surface == Surface.Water)
        {
            return false;
        }

        if (x.Unit != null)
        {
            return false;
        }

        return true;
    }
}