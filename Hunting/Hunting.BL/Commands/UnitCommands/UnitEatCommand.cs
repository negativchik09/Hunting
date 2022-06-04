using Hunting.BL.Abstractions;
using Hunting.BL.Commands.Contracts;
using Hunting.BL.Enum;
using Hunting.BL.Matrix;
using Hunting.BL.Special;
using Hunting.BL.Units;

namespace Hunting.BL.Commands.UnitCommands;

internal class UnitEatCommand : IUnitCommand<UnitEatContract>
{
    private Func<UnitEatContract, bool> _canExecute;
    private Action<UnitEatContract> _execute;
    public UnitEatCommand(string commandText)
    {
        CommandText = commandText;

        _canExecute = contract =>
        {
            return contract.unit.CanEat();
        };
        _execute = contract =>
        {
            contract.unit.Eat();
        };
    }

    public Func<UnitEatContract, bool> CanExecute => _canExecute;

    public Action<UnitEatContract> Execute => _execute;

    public UnitCommandExecutionResult State { get; private set; }

    public string CommandText { get; }
}
