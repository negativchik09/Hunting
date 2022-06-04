using Hunting.BL.Commands.Contracts;
using Hunting.BL.Commands.User;

namespace Hunting.BL.Special;

public class CommandParser // TODO: Add ParsingResultEnum. Validate parameters types
{
    private const string CreateUnitCommandText = "create_unit";
    private const string RemoveUnitCommandText = "remove_unit";
    private const string ChangeSurfaceCommandText = "change_surface";

    public ParsingResult Parse(string commandText)
    {
        string[] command = commandText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        switch (command[0])
        {
            case CreateUnitCommandText:
                return CreateUnitCommandParse(command[1..^1], commandText);
            case RemoveUnitCommandText:
                return RemoveUnitCommandParse(command[1..^1], commandText);
            default:
                return new ParsingResult(false);
        }
    }

    private ParsingResult RemoveUnitCommandParse(string[] parameters, string commandText)
    {
        switch (parameters.Length)
        {
            case 2:
            {
                return new ParsingResult(true);
            }
            default:
            {
                throw new ArgumentOutOfRangeException();
            }
        }
    }

    private ParsingResult CreateUnitCommandParse(string[] parameters, string fullText)
    {
        if (parameters.Length != 4)
        {
            return new ParsingResult(false);
        }
        
        var command = new CreateUnitCommand(fullText);
        var contract = new CreateUnitContract(
            Convert.ToInt32(parameters[0]),
            Convert.ToInt32(parameters[1]),
            parameters[2],
            parameters[3]
        );

        return new ParsingResult(true, command, contract);
    }
}