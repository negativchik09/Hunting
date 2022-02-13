using Hunting.BL.Matrix;

namespace Hunting.BL.Special;

public class MapUpdateEventParameters
{
    public MapUpdateEventParameters(IEnumerable<Node> nodes)
    {
        Nodes = nodes;
    }
    public IEnumerable<Node> Nodes { get; }
}