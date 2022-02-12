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

    public static IEnumerable<Node> GetNeighbouringNodes(Node node, NeighbourType type)
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
}