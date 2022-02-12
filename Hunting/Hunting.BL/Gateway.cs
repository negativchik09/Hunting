using Hunting.BL.Matrix;
using Hunting.BL.Special;
using Newtonsoft.Json;
using Timer = System.Timers.Timer;

namespace Hunting.BL;

public class Gateway
{
    private Timer _timer;

    public Gateway(Timer timer)
    {
        _timer = timer;
        _timer.AutoReset = true;
        _timer.Elapsed += (sender, args) => ExecuteOneTurn();
    }

    public IEnumerable<Node> Nodes => NodeAggregator.Nodes;

    public void ExecuteOneTurn()
    {
        throw new NotImplementedException();
    }

    public void StartContinuousExecution(int seconds)
    {
        _timer.Interval = seconds * 1000;
        _timer.Start();
    }

    public void StopContinuousExecution()
    {
        _timer.Stop();
    }

    public bool LoadMap(string jsonContent)
    {
        _timer.Stop();
        
        IEnumerable<Node>? nodes;
        
        try
        {
            nodes = JsonConvert.DeserializeObject<IEnumerable<Node>>(jsonContent);
        }
        catch (Exception e)
        {
            return false;
        }

        if (nodes == null)
        {
            throw new NullReferenceException();
        }
        
        IEnumerable<Node> nodeList = nodes as Node[] ?? nodes.ToArray();
        
        if (nodeList.Count() != NodeAggregator.MatrixSize * NodeAggregator.MatrixSize)
        {
            return false;
        }
        
        NodeAggregator.NodeList = nodeList;
        OnMapUpdated(new MapUpdateEventParameters(null, NodeAggregator.Nodes));
        
        return true;
    }

    public string SaveMap()
    {
        return JsonConvert.SerializeObject(NodeAggregator.NodeList);
    }

    public void UseDefaultMap()
    {
        NodeAggregator.NodeList = NodeAggregator.DefaultNodes;
    }

    public event EventHandler<MapUpdateEventParameters>? MapUpdated;

    protected void OnMapUpdated(MapUpdateEventParameters e)
    {
        MapUpdated?.Invoke(this, e);
    }
}