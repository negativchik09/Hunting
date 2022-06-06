using Hunting.BL.Abstractions;
using Hunting.BL.Matrix;

namespace Hunting.BL.Commands.Contracts;

public record MoveUnitContract(Unit Unit, Node EndNode) : IContract;
