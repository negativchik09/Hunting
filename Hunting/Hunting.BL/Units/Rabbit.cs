using Hunting.BL.Abstractions;
using Hunting.BL.Commands.Contracts;
using Hunting.BL.Commands.UnitCommands;
using Hunting.BL.Commands.User;
using Hunting.BL.Enum;
using Hunting.BL.Matrix;
using Hunting.BL.Special;

namespace Hunting.BL.Units;

public class Rabbit : Unit
{
    internal Rabbit(string name, Node? node) : base(20, name, 20, node, nameof(Rabbit), 360, 4)
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
        var canStep = Pathfinder.Fow(this).ToList();
        var enemy = canStep
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
                return StepOnRandomInFow(canStep);
            }

            return new MoveUnitCommand($"{UnitType} {Name} tried to escape from enemy to {target.X}:{target.Y}")
            {
                Contract = new MoveUnitContract(this, target)
            };
        }

        if (Hunger >= 10) return StepOnRandomInFow(canStep);
        
        if (CanEat())
        {
            return new UnitEatCommand($"{UnitType} {Name} ate grass at {Node.X}:{Node.Y} and changed surface to ground")
            {
                Contract = new UnitEatContract(this)
            };
        }
        
        var grass = canStep
            .FirstOrDefault(x => x.Surface == Surface.Grass);

        if (grass != null)
        {
            return new MoveUnitCommand($"{UnitType} {Name} found grass at {grass.X}:{grass.Y} and on it's way to it")
            {
                Contract = new MoveUnitContract(this, grass)
            };
        }

        return StepOnRandomInFow(canStep);
    }

    public override void Die()
    {
        Node.Meat.Add(new Meat()
        {
            Amount = 1,
            HungerRegen = 80,
            TurnsBeforeDispose = 15
        });
        base.Die();
    }
}