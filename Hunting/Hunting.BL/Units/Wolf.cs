using Hunting.BL.Abstractions;
using Hunting.BL.Enum;
using Hunting.BL.Matrix;

namespace Hunting.BL.Units;

public class Wolf : Unit
{ 
    internal Wolf(string name, Node? node) : base(100, name, 100, node, nameof(Wolf))
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