using Hunting.BL.Abstractions;
using Hunting.BL.Commands.User;
using Hunting.BL.Commands.UnitCommands;
using Hunting.BL.Enum;
using Hunting.BL.Matrix;

namespace Hunting.BL.Special;

public class CommandExecutor
{
    private static CommandExecutor _commandExecutor;
    public static CommandExecutor Instance
    {
        get => _commandExecutor;
    }

    static CommandExecutor()
    {
        _commandExecutor = new CommandExecutor();
    }

    private CommandExecutor()
    {
        TurnNumber = 0;
        _queue = new PriorityQueue<ICommand, int>();
    }

    public int TurnNumber { get; private set; }

    private PriorityQueue<ICommand, int> _queue;

    public IEnumerable<KeyValuePair<string, bool>> MakeOneTurn()
    {
        TurnNumber += 1;
        foreach (var x in NodeAggregator.NodeList.Where(x =>
                     x.Surface == Surface.Ground && x.TurnsAfterGrassEating != -1))
        {
            x.TurnsAfterGrassEating += 1;
        }

        var continueList = new List<ICommand>();
        
        while (_queue.Count > 0)
        {
            var command = _queue.Dequeue();

            KeyValuePair<string, bool> pair = default;

            switch (command)
            {
                case ChangeSurfaceCommand userCommand:
                {
                    if (userCommand.CanExecute(userCommand.Contract))
                    {
                        userCommand.Execute(userCommand.Contract);
                    }

                    pair = new KeyValuePair<string, bool>(userCommand.CommandText,
                        userCommand.State == UserCommandExecutionResult.Executed);
                    break;
                }
                case CreateUnitCommand userCommand:
                {
                    if (userCommand.CanExecute(userCommand.Contract))
                    {
                        userCommand.Execute(userCommand.Contract);
                    }

                    pair = new KeyValuePair<string, bool>(userCommand.CommandText,
                        userCommand.State == UserCommandExecutionResult.Executed);
                    break;
                }
                case RemoveUnitCommand userCommand:
                {
                    if (userCommand.CanExecute(userCommand.Contract))
                    {
                        userCommand.Execute(userCommand.Contract);
                    }

                    pair = new KeyValuePair<string, bool>(userCommand.CommandText,
                        userCommand.State == UserCommandExecutionResult.Executed);
                    break;
                }
                case MoveUnitCommand unitCommand:
                {
                    if (unitCommand.CanExecute(unitCommand.Contract))
                    {
                        unitCommand.Execute(unitCommand.Contract);
                    }

                    switch (unitCommand.State)
                    {
                        case UnitCommandExecutionResult.Executing:
                            pair = new KeyValuePair<string, bool>(unitCommand.CommandText, true);
                            continueList.Add(unitCommand);
                            Unit.UnitIsHasCommandDict[unitCommand.CommandUnit] = true;
                            break;
                        case UnitCommandExecutionResult.Executed:
                            pair = new KeyValuePair<string, bool>(unitCommand.CommandText, true);
                            Unit.UnitIsHasCommandDict[unitCommand.CommandUnit] = false;
                            break;
                        case UnitCommandExecutionResult.UnableExecute:
                            pair = new KeyValuePair<string, bool>(unitCommand.CommandText, false);
                            if (Unit.Units.Contains(unitCommand.CommandUnit))
                            {
                                Unit.UnitIsHasCommandDict[unitCommand.CommandUnit] = false;
                            }
                            break;
                        case UnitCommandExecutionResult.None:
                            throw new ArgumentException();
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                }
                case UnitAttackCommand unitCommand:
                {
                    if (unitCommand.CanExecute(unitCommand.Contract))
                    {
                        unitCommand?.Execute(unitCommand.Contract);
                    }

                    switch (unitCommand?.State)
                    {
                        case UnitCommandExecutionResult.Executing:
                            continueList.Add(unitCommand);
                            pair = new KeyValuePair<string, bool>(unitCommand.CommandText, true);
                            Unit.UnitIsHasCommandDict[unitCommand.CommandUnit] = true;
                            break;
                        case UnitCommandExecutionResult.Executed:
                            pair = new KeyValuePair<string, bool>(unitCommand.CommandText, true);
                            Unit.UnitIsHasCommandDict[unitCommand.CommandUnit] = false;
                            break;
                        case UnitCommandExecutionResult.UnableExecute:
                            pair = new KeyValuePair<string, bool>(unitCommand.CommandText, false);
                            if (Unit.Units.Contains(unitCommand.Contract.attackedUnit))
                            {
                                Unit.UnitIsHasCommandDict[unitCommand.Contract.attackedUnit] = false;
                            }
                            if (Unit.Units.Contains(unitCommand.Contract.attackingUnit))
                            {
                                Unit.UnitIsHasCommandDict[unitCommand.Contract.attackingUnit] = false;
                            }
                            break;
                        case UnitCommandExecutionResult.None:
                            throw new ArgumentException();
                        case null:
                            throw new ArgumentNullException();
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                }
                case UnitEatCommand unitCommand:
                {
                    if (unitCommand.CanExecute(unitCommand.Contract))
                    {
                        unitCommand?.Execute(unitCommand.Contract);
                    }

                    switch (unitCommand?.State)
                    {
                        case UnitCommandExecutionResult.Executing:
                            continueList.Add(unitCommand);
                            pair = new KeyValuePair<string, bool>(unitCommand.CommandText, true);
                            Unit.UnitIsHasCommandDict[unitCommand.CommandUnit] = true;
                            break;
                        case UnitCommandExecutionResult.Executed:
                            pair = new KeyValuePair<string, bool>(unitCommand.CommandText, true);
                            Unit.UnitIsHasCommandDict[unitCommand.CommandUnit] = false;
                            break;
                        case UnitCommandExecutionResult.UnableExecute:
                            pair = new KeyValuePair<string, bool>(unitCommand.CommandText, false);
                            if (Unit.Units.Contains(unitCommand.CommandUnit))
                            {
                                Unit.UnitIsHasCommandDict[unitCommand.CommandUnit] = false;
                            }
                            break;
                        case UnitCommandExecutionResult.None:
                            throw new ArgumentException();
                        case null:
                            throw new ArgumentNullException();
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                }
                case RestUnitCommand unitCommand:
                {
                    if (unitCommand.CanExecute(unitCommand.Contract))
                    {
                        unitCommand?.Execute(unitCommand.Contract);
                    }

                    switch (unitCommand?.State)
                    {
                        case UnitCommandExecutionResult.Executing:
                            continueList.Add(unitCommand);
                            pair = new KeyValuePair<string, bool>(unitCommand.CommandText, true);
                            Unit.UnitIsHasCommandDict[unitCommand.CommandUnit] = true;
                            break;
                        case UnitCommandExecutionResult.Executed:
                            pair = new KeyValuePair<string, bool>(unitCommand.CommandText, true);
                            Unit.UnitIsHasCommandDict[unitCommand.CommandUnit] = false;
                            break;
                        case UnitCommandExecutionResult.UnableExecute:
                            pair = new KeyValuePair<string, bool>(unitCommand.CommandText, false);
                            if (Unit.Units.Contains(unitCommand.CommandUnit))
                            {
                                Unit.UnitIsHasCommandDict[unitCommand.CommandUnit] = false;
                            }
                            break;
                        case UnitCommandExecutionResult.None:
                            throw new ArgumentException();
                        case null:
                            throw new ArgumentNullException();
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                }
            }

            var units = Unit.UnitIsHasCommandDict.Keys;

            var unitsWithoutCommand = units.Where(unit => !Unit.UnitIsHasCommandDict[unit]);

            var commandsFromUnits = unitsWithoutCommand.Select(unit => unit.GetNextCommand());
            
            continueList.AddRange(commandsFromUnits);
            
            if (pair.Key == null)
            {
                yield break;
            }
            
            yield return pair;
        }

        foreach (var node in NodeAggregator.NodeList)
        {
            if (!node.Meat.Any()) break;
            foreach (var meat in node.Meat)
            {
                meat.TurnsBeforeDispose -= 1;
            }

            node.Meat = node.Meat.Where(x => x.TurnsBeforeDispose > 0).ToList();
        }

        foreach (var node in NodeAggregator.NodeList.Where(x =>
                     x.Surface == Surface.Ground && x.TurnsAfterGrassEating == 20))
        {
            node.TurnsAfterGrassEating = -1;
            node.Surface = Surface.Grass;
        }
        
        foreach (var continueCommand in continueList)
        {
            AddCommand(continueCommand);
        }
    }

    public void AddCommand(ICommand command)
    {
        if (command is null)
        {
            return;
        }
        _queue.Enqueue(command, GetPriority(command)); // lowest first
    }

    private int GetPriority(ICommand command)
    {
        return command.GetType().Name switch
        {
            nameof(ChangeSurfaceCommand) => 1,
            nameof(CreateUnitCommand) => 1,
            nameof(RemoveUnitCommand) => 1,
            nameof(UnitAttackCommand) => 10,
            nameof(MoveUnitCommand) => 15,
            nameof(UnitEatCommand) => 20,
            nameof(RestUnitCommand) => 25,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}