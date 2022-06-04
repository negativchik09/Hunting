namespace Hunting.BL.Enum;

public enum UserCommandExecutionResult
{
    Executed,
    Valid,
    UnknownCommand,
    ParsingError,
    InvalidCoords,
    InvalidSurface,
    InvalidUnitType,
    InvalidUnitName,
    AlreadyHaveUnitOnNode,
    AlreadyHaveUnitWithThisName,
    NoUnitOnNode,
    NoUnitWithThisName
}