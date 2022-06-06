using Hunting.BL.Abstractions;
using Hunting.BL.Commands.Contracts;
using Hunting.BL.Enum;
using Hunting.BL.Matrix;
using Hunting.BL.Units;

namespace Hunting.BL.Commands.User;

public class CreateUnitCommand : IUserCommand<CreateUnitContract>
{
    private Func<CreateUnitContract, bool> _canExecute;
    private Action<CreateUnitContract> _execute;

    public CreateUnitCommand(string commandText)
    {
        CommandText = commandText;
        
        _canExecute = contract =>
        {
            if (contract.X >= NodeAggregator.MatrixSize ||
                contract.X < 0 ||
                contract.Y >= NodeAggregator.MatrixSize ||
                contract.Y < 0)
            {
                State = UserCommandExecutionResult.InvalidCoords;
                return false;
            }
            
            var node = NodeAggregator.GetNode(contract.X, contract.Y);
            if (node.Unit != null)
            {
                State = UserCommandExecutionResult.AlreadyHaveUnitOnNode;
                return false;
            }

            if (node.Surface is Surface.Tree or Surface.Water)
            {
                State = UserCommandExecutionResult.InvalidSurface;
                return false;
            }

            if (Unit.Units.Select(x => x.Name).Contains(contract.UnitName))
            {
                State = UserCommandExecutionResult.AlreadyHaveUnitWithThisName;
                return false;
            }

            if (contract.UnitType is not nameof(Wolf) and not nameof(Rabbit) and not nameof(Huntsman))
            {
                State = UserCommandExecutionResult.InvalidUnitType;
                return false;
            }

            State = UserCommandExecutionResult.Valid;
            return true;
        };

        _execute = contract =>
        {
            var node = NodeAggregator.GetNode(contract.X, contract.Y);
            Unit unit = contract.UnitType switch
            {
                nameof(Wolf) => new Wolf(contract.UnitName, node),
                nameof(Rabbit) => new Rabbit(contract.UnitName, node),
                nameof(Huntsman) => new Huntsman(contract.UnitName, node),
                _ => throw new ArgumentException(
                    $"{contract.UnitType} is not valid Unit type", contract.UnitType, null)
            };
            node.Unit = unit;
            State = UserCommandExecutionResult.Executed;
        };
    }

    public Func<CreateUnitContract, bool> CanExecute => _canExecute;

    public Action<CreateUnitContract> Execute => _execute;

    public UserCommandExecutionResult State { get; set; }

    public string CommandText { get; }

    public CreateUnitContract Contract { get; set; }
}