using Hunting.BL.Abstractions;
using Hunting.BL.Commands.Contracts;
using Hunting.BL.Commands.UnitCommands;
using Hunting.BL.Enum;
using Hunting.BL.Matrix;

namespace Hunting.BL.Units;

public class Wolf : Unit
{ 
    internal Wolf(string name, Node? node) : base(50, name, 200, node, nameof(Wolf), 360, 5)
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
        var fow = Pathfinder.Fow(this).ToList();
        var prey = fow
           .FirstOrDefault(x => x.Unit is { UnitType: nameof(Rabbit) })?.Unit
            ?? NodeAggregator.NeighbouringNodes(Node, NeighbourType.Diagonal)
            .FirstOrDefault(x => x.Unit is { UnitType: nameof(Rabbit) })?.Unit;

        var enemy = fow
            .FirstOrDefault(x => x.Unit is { UnitType: nameof(Huntsman) })?.Unit;

        Node? target = null;
        
        if (enemy != null)
        {
            var nodes = NodeAggregator.VectorFromNode(
                Node,
                Pathfinder.OppositeDirectionByNodes(Node, enemy.Node))
                .Take(NodesPerTurn);
            foreach (var node in nodes)
            {
                if (NodeAggregator.CanStepOnNode(node))
                {
                    target = node;
                }
            }

            if (target == null)
            {
                return StepOnRandomInFow(fow);;
            }

            return new MoveUnitCommand($"{UnitType} {Name} tried to escape from enemy to {target.X}:{target.Y}")
            {
                Contract = new MoveUnitContract(this, target)
            };
        }

        if (Hunger < 100)
        {
            if (CanEat())
            {
                return new UnitEatCommand($"{UnitType} {Name} ate meat at {Node.X}:{Node.Y}")
                {
                    Contract = new UnitEatContract(this)
                };
            }

            var meat = fow.FirstOrDefault(x => x.Meat.Count != 0);
            
            if (meat != null)
            {
                return new MoveUnitCommand($"{UnitType} {Name} found meat at {meat.X}:{meat.Y} and on it's way to it")
                {
                    Contract = new MoveUnitContract(this, meat)
                };
            }
        }

        if (prey == null) return StepOnRandomInFow(fow);
        
        if (NodeAggregator.NeighbouringNodes(Node, NeighbourType.Diagonal).Contains(prey.Node))
        {
            return new UnitAttackCommand($"{UnitType} {Name} attacks its prey {prey.UnitType} {prey.Name} at {prey.Node.X}:{prey.Node.Y}")
            {
                Contract = new UnitAttackContract(this, prey)
            };
        }
        
        //target = NodeAggregator.NeighbouringNodes(prey.Node, NeighbourType.Diagonal).Where(NodeAggregator.CanStepOnNode).First();

        target = Pathfinder.FindNearestNode(Node, prey.Node, NodesPerTurn);

        return new MoveUnitCommand($"{UnitType} {Name} follows its prey {prey.UnitType} {prey.Name} to {target.X}:{target.Y}")
        {
            Contract = new MoveUnitContract(this, target)
        };

    }

    public override void Die()
    {
        Node.Meat.Add(new Meat()
        {
            Amount = 2,
            HungerRegen = 17,
            TurnsBeforeDispose = 12
        });
        base.Die();
    }
}