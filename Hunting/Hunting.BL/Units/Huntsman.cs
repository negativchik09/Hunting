using Hunting.BL.Abstractions;
using Hunting.BL.Commands.Contracts;
using Hunting.BL.Commands.UnitCommands;
using Hunting.BL.Enum;
using Hunting.BL.Matrix;

namespace Hunting.BL.Units;

public class Huntsman : Unit
{
    internal Huntsman(string name, Node? node) : base(100, name, 100, node, nameof(Huntsman), 360, 5)
    { }

    public override void Eat()
    {
        var nodes = NodeAggregator.NeighbouringNodes(Node, NeighbourType.Diagonal)
            .Where(x => x.Meat.Count > 0);
        foreach (var node in nodes)
        {
            var meat = node.Meat.First();
            Hunger += meat.HungerRegen;
            node.Meat.Remove(meat);
            if (Hunger == 100)
            {
                return;
            }
        }
    }

    public override bool CanEat()
    {
        return NodeAggregator.NeighbouringNodes(Node, NeighbourType.Diagonal)
            .Any(x => x.Meat.Any());
    }

    public override ICommand GetNextCommand()
    {
        var fow = Pathfinder.Fow(this).ToList();
        
        if (Hunger < 50)
        {
            if (CanEat())
            {
                return new UnitEatCommand($"{UnitType} {Name} ate meat at {Node.X}:{Node.Y}")
                {
                    Contract = new UnitEatContract(this)
                };
            }

            var meat = fow.FirstOrDefault(x => x.Meat.Count == 0);

            if (meat != null)
            {
                return new MoveUnitCommand($"{UnitType} {Name} found meat at {meat.X}:{meat.Y} and on it's way to it")
                {
                    Contract = new MoveUnitContract(this, meat)
                };
            }
        }

        var prey = fow
           .FirstOrDefault(x => x.Unit != null && x.Unit.UnitType != nameof(Huntsman))?.Unit;

        if (prey != null)
        {
            return new UnitAttackCommand($"{UnitType} {Name} attacks its prey {prey.UnitType} {prey.Name} at {prey.Node.X}:{prey.Node.Y}")
            {
                Contract = new UnitAttackContract(this, prey)
            };
        }

        return StepOnRandomInFow(fow);
    }

    public override void Die()
    {
        Node.Meat.Add(new Meat()
        {
            Amount = 4,
            HungerRegen = 100,
            TurnsBeforeDispose = 10
        });
        base.Die();
    }
}