using Hunting.BL.Abstractions;
using Hunting.BL.Matrix;

namespace Hunting.BL.Special;

public class HuntingWorkflowEventParameters
{
    public HuntingWorkflowEventParameters(ICommand[] commands, Node[] matrix)
    {
        Commands = commands;
        Matrix = matrix;
    }
    public Node[] Matrix { get; }
    public ICommand[] Commands { get; }
}