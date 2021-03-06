using Hunting.BL.Abstractions;

namespace Hunting.BL.Commands.Contracts;

public record UnitAttackContract(Unit AttackingUnit, Unit AttackedUnit) : IContract;
