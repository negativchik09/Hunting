using Hunting.BL.Matrix;

namespace Hunting.BL.Special;

public class MapUpdateEventParameters
{
    public MapUpdateEventParameters(IEnumerable<Node> nodes)
    {
        Nodes = nodes;
    }
    
    public MapUpdateEventParameters(IEnumerable<Node> nodes, Dictionary<string, bool> dictionary, int turnNumber) : this(nodes)
    {
        Commands = dictionary;
        TurnNumber = turnNumber;
    }
    
    public IEnumerable<Node> Nodes { get; }
    public Dictionary<string, bool>? Commands { get; }
    
    public int TurnNumber { get; }
}