using Hunting.BL.Enum;

namespace Hunting.BL.Abstractions;

public interface IUnitCommand<in TParams> : ICommand<TParams, UnitCommandExecutionResult>
    where TParams : IContract
{

}