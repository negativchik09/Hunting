using Hunting.BL.Abstractions;

namespace Hunting.BL.Commands.Contracts;

public record CreateUnitContract(int X, int Y, string UnitType, string UnitName) : IContract;