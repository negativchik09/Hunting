using Hunting.BL.Abstractions;
using Hunting.BL.Matrix;

namespace Hunting.BL.Special;

public class MapUpdateEventParameters
{
    public MapUpdateEventParameters(IEnumerable<ICommand>? commands, IEnumerable<Node> nodes)
    {
        Commands = commands;
        Nodes = nodes;
    }
    public IEnumerable<Node> Nodes { get; }
    public IEnumerable<ICommand>? Commands { get; }
}