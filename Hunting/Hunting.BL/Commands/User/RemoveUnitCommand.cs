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
                return true;
            }
            State = UserCommandExecutionResult.Valid;
            return true;
        };

        _execute = contract =>
        {
            Func<Unit, bool> func = (unit) =>
                    unit.X == contract.X && unit.Y == contract.Y;

            var unit = Unit.Units.First(func);
            
            Unit.Units.Remove(unit);
            
            unit.Node.Unit = null;
        };

    }

    public Func<RemoveUnitContract, bool> CanExecute => _canExecute;

    public Action<RemoveUnitContract> Execute => _execute;

    public UserCommandExecutionResult State { get; set; }

    public string CommandText { get; private set; }

    public RemoveUnitContract Contract { get; set; }
}

