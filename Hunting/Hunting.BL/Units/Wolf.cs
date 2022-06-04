using Hunting.BL.Abstractions;
using Hunting.BL.Commands.Contracts;
using Hunting.BL.Commands.UnitCommands;
using Hunting.BL.Enum;
using Hunting.BL.Matrix;

namespace Hunting.BL.Units;

public class Wolf : Unit
{ 
    internal Wolf(string name, Node? node) : base(50, name, 40, node, nameof(Wolf), 360, 7)
    { }

    public override void Eat()
    {
        var nodes = NodeAggregator.NeighbouringNodes(Node, NeighbourType.Diagonal)
            .Where(x => x.Meat.Count > 0);
        foreach (var node in nodes)
        {
            Hunger += node.Meat.First().HungerRegen;
            if (Hunger == 40)
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
        UnitIsHasCommandDict[this] = false;
        
        var prey = Pathfinder.Fow(this)
           .FirstOrDefault(x => x.Unit != null && x.Unit.UnitType == nameof(Rabbit))?.Unit;

        var enemy = Pathfinder.Fow(this)
            .FirstOrDefault(x => x.Unit != null && x.Unit.UnitType == nameof(Huntsman))?.Unit;

        if (enemy != null)
        {
            var nodes = NodeAggregator.VectorFromNode(
                Node,
                Pathfinder.OppositeDirectionByNodes(Node, enemy.Node))
                .Take(NodesPerTurn);
            Node target = null;
            foreach (var node in nodes)
            {
                if (NodeAggregator.CanStepOnNode(node))
                {
                    target = node;
                }
            }

            if (target == null)
            {
                return null;
            }

            return new MoveUnitCommand($"{UnitType} {Name} tried to escape from enemy to {target.X}:{target.Y}")
            {
                Contract = new MoveUnitContract(this, target)
            };
        }

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

        if (prey != null)
        {
            if (NodeAggregator.NeighbouringNodes(Node, NeighbourType.Diagonal).Where(NodeAggregator.CanStepOnNode).Contains(prey.Node))
            {
                return new UnitAttackCommand($"{UnitType} {Name} attacks its prey {prey.UnitType} {prey.Name} at {prey.Node.X}:{prey.Node.Y}")
                {
                    Contract = new UnitAttackContract(this, prey)
                };
            }
            Node target = NodeAggregator.NeighbouringNodes(Node, NeighbourType.Diagonal).Where(NodeAggregator.CanStepOnNode).First();
            return new MoveUnitCommand($"{UnitType} {Name} follows its prey {prey.UnitType} {prey.Name} to {target.X}:{target.Y}")
            {
                Contract = new MoveUnitContract(this, target)
            };
        }

        var fowPoints = Pathfinder.Fow(this)
            .Where(NodeAggregator.CanStepOnNode)
            .ToList();

        var randIndex = new Random().Next(0, fowPoints.Count);

        var randPoint = fowPoints[randIndex];

        return new MoveUnitCommand(
            $"{UnitType} {Name} hasn`t found meat or enemies in it's FOW at {Node.X}:{Node.Y} and moving to {randPoint.X}:{randPoint.Y}")
        {
            Contract = new MoveUnitContract(this, randPoint)
        };
    }

    public override void Die()
    {
        Node.Meat.Add(new Meat()
        {
            Amount = 2,
            HungerRegen = 17,
            TurnsBeforeDispose = 4
        });
        base.Die();
    }
}