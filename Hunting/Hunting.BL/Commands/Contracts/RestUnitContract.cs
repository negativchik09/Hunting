using Hunting.BL.Abstractions;

namespace Hunting.BL.Commands.Contracts;

public class RestUnitContract : IContract
{
    public Unit Unit { get; set; }
}