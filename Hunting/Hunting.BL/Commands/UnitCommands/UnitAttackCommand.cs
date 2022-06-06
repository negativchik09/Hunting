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
            CommandUnit = contract.AttackingUnit;
            if (!Unit.Units.Contains(contract.AttackingUnit))
            {
                State = UnitCommandExecutionResult.UnableExecute;
                return false;
            }
            if (!Unit.Units.Contains(contract.AttackedUnit))
            {
                State = UnitCommandExecutionResult.UnableExecute;
                return false;
            }
            switch (contract.AttackingUnit.UnitType)
            {
                case nameof(Wolf):
                    return NodeAggregator.IsNeighbouring(contract.AttackingUnit.Node, contract.AttackedUnit.Node, NeighbourType.Diagonal);
                case nameof(Huntsman):
                    return Pathfinder.Fow(contract.AttackingUnit).Contains(contract.AttackedUnit.Node);
                default:
                    throw new ArgumentException(
                    $"{contract.AttackingUnit.UnitType} is not valid Unit type", contract.AttackingUnit.UnitType, null);
            }
        };
        
        _execute = contract =>
        {
            if (!_canExecute(contract)) return;
            CommandUnit = contract.AttackingUnit;
            contract.AttackingUnit.Hunger -= 5;
            switch (contract.AttackingUnit.UnitType)
            {
                case nameof(Wolf):
                    contract.AttackedUnit.Hp -= 15;
                    contract.AttackedUnit.Hunger /= 2;
                    State = UnitCommandExecutionResult.Executed;
                    return;
                case nameof(Huntsman):
                    contract.AttackedUnit.Hp -= 999;
                    State = UnitCommandExecutionResult.Executed;
                    return;
                default:
                    throw new ArgumentException(
                    $"{contract.AttackingUnit.UnitType} is not valid Unit type", contract.AttackingUnit.UnitType, null);
            }
        };
    }

    public Func<UnitAttackContract, bool> CanExecute => _canExecute;

    public Action<UnitAttackContract> Execute => _execute;

    public UnitCommandExecutionResult State { get; set; }

    public string CommandText { get; }

    public UnitAttackContract Contract { get; set; }
    public Unit CommandUnit { get; private set; }
}
