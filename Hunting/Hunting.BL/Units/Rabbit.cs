using Hunting.BL.Abstractions;
using Hunting.BL.Commands.Contracts;
using Hunting.BL.Commands.UnitCommands;
using Hunting.BL.Enum;
using Hunting.BL.Matrix;

namespace Hunting.BL.Units;

public class Rabbit : Unit
{
    internal Rabbit(string name, Node? node) : base(20, name, 20, node, nameof(Rabbit), 360, 3)
    { }

    public override void Eat()
    {
        Hunger += 10;
        Node.Surface = Surface.Ground;
        Node.TurnsAfterGrassEating = 0;
    }

    public override bool CanEat()
    {
        return Node.Surface == Surface.Grass;
    }

    public override ICommand GetNextCommand()
    {
        UnitIsHasCommandDict[this] = true;
        var enemy = Pathfinder.Fow(this)
            .FirstOrDefault(x => x.Unit != null && x.Unit.UnitType != nameof(Rabbit))?.Unit;
        
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
                return StepOnRandomInFow();
            }

            return new MoveUnitCommand($"{UnitType} {Name} tried to escape from enemy to {target.X}:{target.Y}")
            {
                Contract = new MoveUnitContract(this, target)
            };
        }

        if (Hunger >= 10) return StepOnRandomInFow();
        
        if (CanEat())
        {
            return new UnitEatCommand($"{UnitType} {Name} ate grass at {Node.X}:{Node.Y} and changed surface to ground")
            {
                Contract = new UnitEatContract(this)
            };
        }
        
        var grass = Pathfinder.Fow(this)
            .FirstOrDefault(x => x.Surface == Surface.Grass);

        if (grass != null)
        {
            return new MoveUnitCommand($"{UnitType} {Name} found grass at {grass.X}:{grass.Y} and on it's way to it")
            {
                Contract = new MoveUnitContract(this, grass)
            };
        }

        return StepOnRandomInFow();
    }

    private ICommand StepOnRandomInFow()
    {
        var fowPoints = Pathfinder.Fow(this);
        var filtered = fowPoints.Where(NodeAggregator.CanStepOnNode)
            .ToList();

        if (!filtered.Any())
        {
            return new RestUnitCommand(
                $"{UnitType} {Name} hasn`t found grass or enemies in it's FOW at {Node.X}:{Node.Y} and decided to rest");
        }
        
        var randIndex = new Random().Next(0, filtered.Count);
        
        var randPoint = filtered[randIndex];
        
        return new MoveUnitCommand(
            $"{UnitType} {Name} hasn`t found grass or enemies in it's FOW at {Node.X}:{Node.Y} and moving to {randPoint.X}:{randPoint.Y}")
        {
            Contract = new MoveUnitContract(this, randPoint)
        };
    }

    public override void Die()
    {
        Node.Meat.Add(new Meat()
        {
            Amount = 1,
            HungerRegen = 15,
            TurnsBeforeDispose = 5
        });
        base.Die();
    }
    
    
}