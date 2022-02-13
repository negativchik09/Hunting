using Hunting.BL.Abstractions;
using Hunting.BL.Enum;
using Hunting.BL.Matrix;

namespace Hunting.BL.Units;

public class Huntsman : Unit
{
    internal Huntsman(string name, Node? node) : base(100, name, 100, node, nameof(Huntsman))
    { }

    public override UnitCommandExecutionResult Eat()
    {
        throw new NotImplementedException();
    }
}