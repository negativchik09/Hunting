using Hunting.BL.Matrix;

namespace Hunting.BL.Special;

public class MovingPathFindResult
{
    public bool CanMove { get; init; }
    public Queue<Node> Steps { get; init; }
}