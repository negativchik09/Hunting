using Hunting.BL.Abstractions;

namespace Hunting.BL.Commands.Contracts;

public record RestUnitContract(Unit Unit) : IContract
{ }