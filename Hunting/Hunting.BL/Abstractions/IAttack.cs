using Hunting.BL.Enum;
using Hunting.BL.Matrix;

namespace Hunting.BL.Abstractions;

public interface IAttack
{
    public UnitCommandExecutionResult Attack(Node node);
}