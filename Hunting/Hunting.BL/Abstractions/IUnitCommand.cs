using Hunting.BL.Enum;

namespace Hunting.BL.Abstractions;

public interface IUnitCommand<in TParams> : ICommand<TParams, UnitCommandExecutionResult>
    where TParams : IContract
{
    Unit Unit { get; init; }
    int Priority { get; }
    double Duration { get; internal set; }
}