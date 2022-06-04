using Hunting.BL.Abstractions;
using Hunting.BL.Commands.Contracts;
using Hunting.BL.Enum;

namespace Hunting.BL.Commands.User;

public class ChangeSurfaceCommand : IUserCommand<ChangeSurfaceContract>
{
    public string CommandText { get; }
    public Func<ChangeSurfaceContract, bool> CanExecute { get; }
    public Action<ChangeSurfaceContract> Execute { get; }
    public UserCommandExecutionResult State { get; }
}