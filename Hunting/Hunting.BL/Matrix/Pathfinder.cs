﻿using Hunting.BL.Abstractions;
using Hunting.BL.Enum;
using Hunting.BL.Special;

namespace Hunting.BL.Matrix;

internal static class Pathfinder
{
    public static IEnumerable<Node> Fow(Unit unit)
    {
        IEnumerable<Node> nodes = NodeAggregator.Segment(
            unit.Node,
            unit.VisibilityRange);

        var start = unit.Node;
        var resultList = new HashSet<Node>();
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

                if (nodes.Contains(current))
                {
                    resultList.Add(current);
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

        return resultList;
    }

    public static Direction OppositeDirectionByNodes(Node first, Node second)
    {
        var xRange = second.X - first.X;
        var yRange = second.Y - first.Y;
        
        if (Math.Abs(xRange) >= Math.Abs(yRange))
        {
            return yRange > 0 ? Direction.Top : Direction.Bot;
        }

        return xRange > 0 ? Direction.Left : Direction.Right;
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
        var result = FindWay(unit.Node, node, unit.VisibilityRange);
        var steps = () =>
        {
            Queue<Node> nodes = new Queue<Node>();
            Node? nextStep = null;
            
            while (result?.TryPop(out nextStep) == true)
            {
                EnqueueIfNotNull(nextStep, nodes);
            }

            return nodes;
        };
        return new MovingPathFindResult
        {
            CanMove = result != null,
            Steps = steps()
        };
    }
    
    private static Stack<Node>? FindWay(Node start, Node end, int range)
    {
        if (!NodeAggregator.CanStepOnNode(end))
        {
            return null;
        }
        var distanceDict = NodeAggregator.Segment(start, range)
            .Where(NodeAggregator.CanStepOnNode)
            .ToDictionary(x => x, x => -1);
        SetDistances(start, distanceDict);
        var way = new Stack<Node>();
        if (distanceDict[end] == -1)
        {
            return null;
        }
        var current = end;
        for (var i = 0; i < distanceDict[end]; i++)
        {
            way.Push(current);
            current = distanceDict.Keys
                .Intersect(NodeAggregator.NeighbouringNodes(current, NeighbourType.Side))
                .First(x => distanceDict[current] - 1 == distanceDict[x]);
        }

        return way;
    }

    private static void SetDistances(Node start, IDictionary<Node, int> dictionary)
    {
        var list = new List<Node>() { start };
        var visited = new List<Node>();
        for (var i = 0; list.Count > 0 && i < 11; i++)
        {
            foreach (var node in list)
            {
                dictionary[node] = i;
                visited.Add(node);
            }

            IEnumerable<Node> nodes = list
                .Select(node => NodeAggregator.NeighbouringNodes(node, NeighbourType.Side)
                    .Where(NodeAggregator.CanStepOnNode)
                    .Except(visited))
                .Aggregate((x, y) => x.Concat(y));

            list.Clear();
            list.AddRange(nodes);
        }
    }
}