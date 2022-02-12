using Hunting.BL.Abstractions;

namespace Hunting.BL.Units;

public class Huntsman : IUnit
{
    public int Hp { get; }
    public string Name { get; }
}