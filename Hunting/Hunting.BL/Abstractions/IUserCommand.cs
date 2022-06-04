using Hunting.BL.Enum;

namespace Hunting.BL.Abstractions;

public interface IUserCommand<TParams> : ICommand<TParams, UserCommandExecutionResult> 
    where TParams : IContract
{ }