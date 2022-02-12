using Hunting.BL.Enum;

namespace Hunting.BL.Abstractions;

public interface IUserCommand : ICommand
{
    UserCommandExecutionResult Result { get; internal set; }
    string CommandText { get; }
}