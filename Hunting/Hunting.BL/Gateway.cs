using Hunting.BL.Enum;
using Hunting.BL.Matrix;
using Hunting.BL.Special;
using Newtonsoft.Json;
using Timer = System.Timers.Timer;

namespace Hunting.BL;

public class Gateway
{
    private readonly Timer _timer;

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

        IEnumerable<Node> nodeList = nodes as Node[] ?? nodes.ToArray();
        
        if (nodeList.Count() != NodeAggregator.MatrixSize * NodeAggregator.MatrixSize)
        {
            return false;
        }
        
        NodeAggregator.NodeList = nodeList;
        OnMapUpdated(new MapUpdateEventParameters(NodeAggregator.Nodes));
        
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
                    Meat = new List<Meat>(),
                    Surface = Surface.Grass,
                    Unit = null,
                    X = j,
                    Y = i
                });
            }
        }
        
        return JsonConvert.SerializeObject(nodes, Formatting.Indented);
    }

    public void AddCommand(string command)
    {
        var parsingResult = CommandParser.Parse(command);
        CommandExecutor.Instance.AddCommand(parsingResult.Command);
        return;
    }

    public void AddCommandRange(string commands)
    {
        string[] splitted_commands = commands.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        foreach (string command in splitted_commands)
        {
            AddCommand(command);
        }
        return;
    }

    public event EventHandler<MapUpdateEventParameters>? MapUpdated;

    protected void OnMapUpdated(MapUpdateEventParameters e)
    {
        MapUpdated?.Invoke(this, e);
    }
}