using Hunting.BL.Abstractions;
using Hunting.BL.Enum;

namespace Hunting.BL.Units;

public class Huntsman : Unit
{
    internal Huntsman()
    {
        UnitType = nameof(Huntsman);
    }

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