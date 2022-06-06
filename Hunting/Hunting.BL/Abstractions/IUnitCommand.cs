using Hunting.BL.Enum;

namespace Hunting.BL.Abstractions;

public interface IUnitCommand<TParams> : ICommand<TParams, UnitCommandExecutionResult>
    where TParams : IContract
{
    Unit CommandUnit { get; }
}