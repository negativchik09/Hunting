using Hunting.BL.Abstractions;
using Hunting.BL.Commands.Contracts;
using Hunting.BL.Enum;
using Hunting.BL.Matrix;

namespace Hunting.BL.Commands.User;

public class ChangeSurfaceCommand : IUserCommand<ChangeSurfaceContract>
{
    private Func<ChangeSurfaceContract, bool> _canExecute;
    private Action<ChangeSurfaceContract> _execute;
    public ChangeSurfaceCommand(string commandText)
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
            State = UserCommandExecutionResult.Valid;
            return true;
        };

        _execute = contract =>
        {
            int surfaceIndex;
            switch (contract.SurfaceType)
            {
                case "Grass":
                    surfaceIndex = 0;
                    break;
                case "Water":
                    surfaceIndex = 1;
                    break;
                case "Ground":
                    surfaceIndex = 2;
                    break;
                case "Stones":
                    surfaceIndex = 3;
                    break;
                case "Tree":
                    surfaceIndex = 4;
                    break;
                default: throw new ArgumentException($"{contract.SurfaceType} is not valid surface type", contract.SurfaceType, null);
            }
            NodeAggregator.GetNode(contract.X, contract.Y).Surface = (Surface)surfaceIndex;
            State = UserCommandExecutionResult.Executed;
        };
    }
    public string CommandText { get; }
    public Func<ChangeSurfaceContract, bool> CanExecute => _canExecute;
    public Action<ChangeSurfaceContract> Execute => _execute;
    public UserCommandExecutionResult State { get; set; }
    public ChangeSurfaceContract Contract { get; set; }
}