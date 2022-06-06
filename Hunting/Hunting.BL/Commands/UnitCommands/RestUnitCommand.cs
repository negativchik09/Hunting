using Hunting.BL.Abstractions;
using Hunting.BL.Commands.Contracts;
using Hunting.BL.Enum;
using Hunting.BL.Matrix;
using Hunting.BL.Special;
using Hunting.BL.Units;

namespace Hunting.BL.Commands.UnitCommands;

public class RestUnitCommand : IUnitCommand<RestUnitContract>
{
    private Func<RestUnitContract, bool> _canExecute;
    private Action<RestUnitContract> _execute;

    public RestUnitCommand(string commandText)
    {
        CommandText = commandText;
        _canExecute = contract =>
        {
            CommandUnit = contract.Unit;
            if (Unit.Units.Contains(contract.Unit)) return true;
            State = UnitCommandExecutionResult.UnableExecute;
            return false;
        };
        _execute = contract =>
        {
            CommandUnit = contract.Unit;
            CommandUnit.Hunger -= 41; // TODO: Change to 1
            State = UnitCommandExecutionResult.Executed;
        };
    }

    public Func<RestUnitContract, bool> CanExecute => _canExecute;

    public Action<RestUnitContract> Execute => _execute;

    public UnitCommandExecutionResult State { get; set; }

    public string CommandText { get; }
    public RestUnitContract Contract { get; set; }
    
    public Unit CommandUnit { get; private set; }
}
