using Hunting.BL.Abstractions;
using Hunting.BL.Commands.Contracts;
using Hunting.BL.Enum;
using Hunting.BL.Matrix;
using Hunting.BL.Special;
using Hunting.BL.Units;

namespace Hunting.BL.Commands.UnitCommands;

public class MoveUnitCommand : IUnitCommand<MoveUnitContract>
{
    private Func<MoveUnitContract, bool> _canExecute;
    private Action<MoveUnitContract> _execute;

    private MovingPathFindResult MovingPathfindRes;
    public MoveUnitCommand(string commandText)
    {
        CommandText = commandText;
        _canExecute = contract =>
        {
            CommandUnit = contract.Unit;
            if (!Unit.Units.Contains(contract.Unit))
            {
                State = UnitCommandExecutionResult.UnableExecute;
                return false;
            }
            MovingPathfindRes = Pathfinder.FindPath(contract.Unit.Node, contract.EndNode, contract.Unit.VisibilityRange);
            if (MovingPathfindRes.CanMove) return MovingPathfindRes.CanMove;
            State = UnitCommandExecutionResult.UnableExecute;
            return MovingPathfindRes.CanMove;
        };
        _execute = contract =>
        {
            if (!_canExecute(contract)) return;
            CommandUnit = contract.Unit;
            int numberOfSteps = contract.Unit.NodesPerTurn;
            for (int i = 0; i< numberOfSteps; i++)
            {
                contract.Unit.Step(MovingPathfindRes.Steps.Dequeue());
                if (contract.Unit.Node == contract.EndNode || !Unit.Units.Contains(contract.Unit))
                {
                    State = UnitCommandExecutionResult.Executed;
                    return;
                }
                else
                {
                    State = UnitCommandExecutionResult.Executing;
                }
            }
        };
    }

    public Func<MoveUnitContract, bool> CanExecute => _canExecute;

    public Action<MoveUnitContract> Execute => _execute;

    public UnitCommandExecutionResult State { get; set; }

    public string CommandText { get; }
    public MoveUnitContract Contract { get; set; }
    
    public Unit CommandUnit { get; private set; }
}
