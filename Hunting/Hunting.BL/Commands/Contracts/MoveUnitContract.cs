using Hunting.BL.Abstractions;
using Hunting.BL.Matrix;

namespace Hunting.BL.Commands.Contracts;

public record MoveUnitContract(Unit unit, Node endNode) : IContract;
