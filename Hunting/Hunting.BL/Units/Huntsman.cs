using Hunting.BL.Abstractions;
using Hunting.BL.Enum;
using Hunting.BL.Matrix;

namespace Hunting.BL.Units;

public class Huntsman : Unit
{
    internal Huntsman(string name, Node? node) : base(100, name, 100, node, nameof(Huntsman))
    { }

    public override void Eat()
    {
        throw new NotImplementedException();
    }

    public override bool CanEat()
    {
        throw new NotImplementedException();
    }
}