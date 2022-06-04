using Hunting.BL.Abstractions;

namespace Hunting.BL.Commands.Contracts;

public record RemoveUnitContract(int X, int Y) : IContract {}