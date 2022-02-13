using Hunting.BL.Enum;
using Hunting.BL.Matrix;
using Hunting.BL.Special;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Timer = System.Timers.Timer;

namespace Hunting.BL;

public class Gateway
{
    private Timer _timer;

    public Gateway()
    {
        _timer = new Timer();
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

        if (nodes.Count() != NodeAggregator.MatrixSize * NodeAggregator.MatrixSize)
        {
            return false;
        }
        
        NodeAggregator.NodeList = nodes;
        OnMapUpdated(new MapUpdateEventParameters(null, NodeAggregator.Nodes));
        
        return true;
    }

    public string SaveMap()
    {
        return JsonConvert.SerializeObject(NodeAggregator.NodeList, Formatting.Indented);
    }

    public string GetDefaultMap()
    {
        var nodes = new List<Node?>();
        for (int i = 0; i < NodeAggregator.MatrixSize; i++)
        {
            for (int j = 0; j < NodeAggregator.MatrixSize; j++)
            {
                nodes.Add(new Node()
                {
                    Meat = Array.Empty<Meat>(),
                    Surface = Surface.Grass,
                    Unit = null,
                    X = j,
                    Y = i
                });
            }
        }
        
        return JsonConvert.SerializeObject(nodes, Formatting.Indented);
    }

    public event EventHandler<MapUpdateEventParameters>? MapUpdated;

    protected void OnMapUpdated(MapUpdateEventParameters e)
    {
        MapUpdated?.Invoke(this, e);
    }
}