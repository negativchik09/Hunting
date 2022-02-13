using Hunting.BL.Enum;

namespace Hunting.BL.Abstractions;

public interface IUnitCommand<in TParams> : ICommand<TParams, UnitCommandExecutionResult>
{
    Unit Unit { get; init; }
    int Priority { get; }
    double Duration { get; internal set; }
}