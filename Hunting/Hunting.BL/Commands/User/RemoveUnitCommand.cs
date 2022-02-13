using Hunting.BL.Abstractions;
using Hunting.BL.Commands.Contracts;
using Hunting.BL.Enum;
using Hunting.BL.Matrix;
using Hunting.BL.Units;

namespace Hunting.BL.Commands.User;

public class RemoveUnitCommand : IUserCommand<RemoveUnitContract>
{
    private Func<RemoveUnitContract, bool> _canExecute;
    private Action<RemoveUnitContract> _execute;

    public RemoveUnitCommand(string commandText)
    {
        CommandText = commandText;
        _canExecute = contract =>
        {
            if (contract.RemoveUnitContractType == RemoveUnitContractType.ByCoordinates)
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
                if (node.Unit == null)
                {
                    State = UserCommandExecutionResult.NoUnitOnNode;
                    return false;
                }
            }
            else
            {
                if (!Unit.Units.Select(x => x.Name).Contains(contract.UnitName))
                {
                    State = UserCommandExecutionResult.NoUnitWithThisName;
                    return false;
                }
            }
            State = UserCommandExecutionResult.Valid;
            return true;
        };

        _execute = contract => 
        {
            Node node;
            if (contract.RemoveUnitContractType == RemoveUnitContractType.ByCoordinates)
            {
                node = NodeAggregator.GetNode(contract.X, contract.Y);
            }
            else
            {
                node = Unit.Units.First(x => x.Name == contract.UnitName).Node;
            }
            Unit.Units.Remove(node.Unit);
            node.Unit = null;
        };

    }

    public Func<RemoveUnitContract, bool> CanExecute => _canExecute;

    public Action<RemoveUnitContract> Execute => _execute;

    public UserCommandExecutionResult State { get; private set; }

    public string CommandText { get; private set; }
}

