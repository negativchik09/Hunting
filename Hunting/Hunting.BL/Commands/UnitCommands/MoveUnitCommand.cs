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
            CommandUnit = contract.unit;
            if (!Unit.Units.Contains(contract.unit))
            {
                State = UnitCommandExecutionResult.UnableExecute;
                return false;
            }
            MovingPathfindRes = Pathfinder.FindPath(contract.unit, contract.endNode);
            if (MovingPathfindRes.CanMove) return MovingPathfindRes.CanMove;
            State = UnitCommandExecutionResult.UnableExecute;
            return MovingPathfindRes.CanMove;
        };
        _execute = contract =>
        {
            if (!_canExecute(contract)) return;
            CommandUnit = contract.unit;
            int numberOfSteps = contract.unit.UnitType switch
            {
                nameof(Wolf) => 3,
                nameof(Rabbit) => 2,
                nameof(Huntsman) => 2,
                _ => throw new ArgumentException(
                    $"{contract.unit.UnitType} is not valid unit type", contract.unit.UnitType, null)
            };
            for (int i = 0; i< numberOfSteps; i++)
            {
                contract.unit.Step(MovingPathfindRes.Steps.Dequeue());
                if (contract.unit.Node == contract.endNode)
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
