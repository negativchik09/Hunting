using Hunting.BL.Commands.Contracts;
using Hunting.BL.Commands.User;
using Hunting.BL.Enum;

namespace Hunting.BL.Special;

public static class CommandParser
{
    private const string CreateUnitCommandText = "create_unit";
    private const string RemoveUnitCommandText = "remove_unit";
    private const string ChangeSurfaceCommandText = "change_surface";

    public static ParsingResult Parse(string commandText)
    {
        string[] command = commandText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        switch (command[0])
        {
            case CreateUnitCommandText:
                return CreateUnitCommandParse(command[1..^1], commandText);
            case RemoveUnitCommandText:
                return RemoveUnitCommandParse(command[1..^1], commandText);
            case ChangeSurfaceCommandText:
                return ChangeSurfaceCommandParse(command[1..^1], commandText);
            default:
                return new ParsingResult(false);
        }
    }

    private static ParsingResult RemoveUnitCommandParse(string[] parameters, string fullText)
    {
        var command = new RemoveUnitCommand(fullText);
        if (parameters.Length != 2)
        {
            command.State = UserCommandExecutionResult.ParsingError;
            return new ParsingResult(false, command, null);
        }
        var contract = new RemoveUnitContract(
            Convert.ToInt32(parameters[0]),
            Convert.ToInt32(parameters[1])
        );

        command.Contract = contract;

        return new ParsingResult(true, command, contract);
    }

    private static ParsingResult CreateUnitCommandParse(string[] parameters, string fullText)
    {
        var command = new CreateUnitCommand(fullText);
        if (parameters.Length != 4)
        {
            return new ParsingResult(false, command, null);
        }
        var contract = new CreateUnitContract(
            Convert.ToInt32(parameters[0]),
            Convert.ToInt32(parameters[1]),
            parameters[2],
            parameters[3]
        );

        command.Contract = contract;

        return new ParsingResult(true, command, contract);
    }
    
    private static ParsingResult ChangeSurfaceCommandParse(string[] parameters, string fullText)
    {
        var command = new ChangeSurfaceCommand(fullText);
        if (parameters.Length != 3)
        {
            return new ParsingResult(false, command, null);
        }
        var contract = new ChangeSurfaceContract(
            Convert.ToInt32(parameters[0]),
            Convert.ToInt32(parameters[1]),
            parameters[2]
        );

        command.Contract = contract;

        return new ParsingResult(true, command, contract);
    }
}