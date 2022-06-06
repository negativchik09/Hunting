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
            Hunger += node.Meat.First().HungerRegen;
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
        if (Hunger < 20)
        {
            if (CanEat())
            {
                return new UnitEatCommand($"{UnitType} {Name} ate meat at {Node.X}:{Node.Y}")
                {
                    Contract = new UnitEatContract(this)
                };
            }

            var meat = Pathfinder.Fow(this).FirstOrDefault(x => x.Meat.Count == 0);

            if (meat != null)
            {
                return new MoveUnitCommand($"{UnitType} {Name} found meat at {meat.X}:{meat.Y} and on it's way to it")
                {
                    Contract = new MoveUnitContract(this, meat)
                };
            }
        }

        var prey = Pathfinder.Fow(this)
           .FirstOrDefault(x => x.Unit != null && x.Unit.UnitType != nameof(Huntsman))?.Unit;

        if (prey != null)
        {
            return new UnitAttackCommand($"{UnitType} {Name} attacks its prey {prey.UnitType} {prey.Name} at {prey.Node.X}:{prey.Node.Y}")
            {
                Contract = new UnitAttackContract(this, prey)
            };
        }

        var fowPoints = Pathfinder.Fow(this)
            .Where(NodeAggregator.CanStepOnNode)
            .ToList();

        if (!fowPoints.Any()) return null;
        
        var randIndex = new Random().Next(0, fowPoints.Count);

        var randPoint = fowPoints[randIndex];

        return new MoveUnitCommand(
            $"{UnitType} {Name} hasn`t found meat or prey in it's FOW at {Node.X}:{Node.Y} and moving to {randPoint.X}:{randPoint.Y}")
        {
            Contract = new MoveUnitContract(this, randPoint)
        };
    }

    public override void Die()
    {
        Node.Meat.Add(new Meat()
        {
            Amount = 4,
            HungerRegen = 20,
            TurnsBeforeDispose = 3
        });
        Node.Unit = null;
        UnitIsHasCommandDict.Remove(this);
    }
}