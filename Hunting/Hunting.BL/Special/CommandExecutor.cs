using Hunting.BL.Abstractions;
using Hunting.BL.Commands.User;
using Hunting.BL.Commands.UnitCommands;

namespace Hunting.BL.Special;

public class CommandExecutor
{
    private static CommandExecutor _commandExecutor;
    public static CommandExecutor Instance
    {
        get => _commandExecutor;
        private set => _commandExecutor = value;
    }

    static CommandExecutor()
    {
        _commandExecutor = new CommandExecutor();
    }

    private CommandExecutor()
    {
        _queue = new PriorityQueue<ICommand, int>();
    }

    private PriorityQueue<ICommand, int> _queue;

    public void AddCommand(ICommand command)
    {
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

            _ => throw new ArgumentOutOfRangeException()
        };
    }
}