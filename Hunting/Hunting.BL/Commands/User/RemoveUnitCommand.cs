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
                if (contract.ByCoords.X >= NodeAggregator.MatrixSize ||
                    contract.ByCoords.X < 0 ||
                    contract.ByCoords.Y >= NodeAggregator.MatrixSize ||
                    contract.ByCoords.Y < 0)
                {
                    State = UserCommandExecutionResult.InvalidCoords;
                    return false;
                }
                var node = NodeAggregator.GetNode(contract.ByCoords.X, contract.ByCoords.Y);
                if (node.Unit == null)
                {
                    State = UserCommandExecutionResult.NoUnitOnNode;
                    return false;
                }
            }
            else
            {
                if (!Unit.Units.Select(x => x.Name).Contains(contract.ByName.UnitName))
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
            Func<Unit, bool> func = contract.RemoveUnitContractType switch
            {
                RemoveUnitContractType.ByCoordinates => (unit) =>
                    unit.X == contract.ByCoords.X && unit.Y == contract.ByCoords.Y,
                RemoveUnitContractType.ByName => (unit) => unit.Name == contract.ByName.UnitName,
                _ => throw new ArgumentOutOfRangeException()
            };

            var unit = Unit.Units.First(func);
            
            Unit.Units.Remove(unit);
            
            unit.Node.Unit = null;
        };

    }

    public Func<RemoveUnitContract, bool> CanExecute => _canExecute;

    public Action<RemoveUnitContract> Execute => _execute;

    public UserCommandExecutionResult State { get; private set; }

    public string CommandText { get; private set; }
}

