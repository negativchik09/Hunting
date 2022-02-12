using Hunting.BL.Enum;

namespace Hunting.BL.Abstractions;

public interface IUnitCommand : ICommand
{
    Unit Unit { get; init; }
    int Priority { get; }
    double Duration { get; internal set; }
    UnitCommandExecutionResult Execute();
}