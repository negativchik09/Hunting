using Hunting.BL.Abstractions;

namespace Hunting.BL.Commands.Contracts;

public record UnitEatContract(Unit Unit) : IContract;
