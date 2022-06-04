using Hunting.BL.Abstractions;
using Hunting.BL.Commands.Contracts;
using Hunting.BL.Enum;
using Hunting.BL.Matrix;
using Hunting.BL.Special;
using Hunting.BL.Units;

namespace Hunting.BL.Commands.UnitCommands;

internal class UnitAttackCommand : IUnitCommand<UnitAttackContract>
{
    private Func<UnitAttackContract, bool> _canExecute;
    private Action<UnitAttackContract> _execute;
    public UnitAttackCommand(string commandText)
    {
        CommandText = commandText;

        _canExecute = contract =>
        {
            switch (contract.attackingUnit.UnitType)
            {
                case nameof(Wolf):
                    return NodeAggregator.IsNeighbouring(contract.attackingUnit.Node, contract.attackedUnit.Node, NeighbourType.Diagonal);
                case nameof(Huntsman):
                    return Pathfinder.Fow(contract.attackingUnit).Contains(contract.attackedUnit.Node);
                default:
                    throw new ArgumentException(
                    $"{contract.attackingUnit.UnitType} is not valid unit type", contract.attackingUnit.UnitType, null);
            }
        };
        _execute = contract =>
        {
            switch (contract.attackingUnit.UnitType)
            {
                case nameof(Wolf):
                    contract.attackedUnit.Hp -= 15;
                    State = UnitCommandExecutionResult.Executed;
                    return;
                case nameof(Huntsman):
                    contract.attackedUnit.Hp -= 999;
                    State = UnitCommandExecutionResult.Executed;
                    return;
                default:
                    throw new ArgumentException(
                    $"{contract.attackingUnit.UnitType} is not valid unit type", contract.attackingUnit.UnitType, null);
            }
        };
    }

    public Func<UnitAttackContract, bool> CanExecute => _canExecute;

    public Action<UnitAttackContract> Execute => _execute;

    public UnitCommandExecutionResult State { get; set; }

    public string CommandText { get; }

    public UnitAttackContract Contract { get; set; }
}
