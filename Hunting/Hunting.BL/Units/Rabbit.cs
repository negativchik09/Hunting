using Hunting.BL.Abstractions;
using Hunting.BL.Enum;
using Hunting.BL.Matrix;

namespace Hunting.BL.Units;

public class Rabbit : Unit
{
    internal Rabbit(string name, Node? node) : base(100, name, 100, node, nameof(Rabbit))
    { }

    public override UnitCommandExecutionResult Step()
    {
        throw new NotImplementedException();
    }

    public override UnitCommandExecutionResult TurnLeft()
    {
        throw new NotImplementedException();
    }

    public override UnitCommandExecutionResult TurnRight()
    {
        throw new NotImplementedException();
    }

    public override UnitCommandExecutionResult Eat()
    {
        throw new NotImplementedException();
    }
}