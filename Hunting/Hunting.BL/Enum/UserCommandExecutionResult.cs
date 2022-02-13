namespace Hunting.BL.Enum;

public enum UserCommandExecutionResult
{
    Executed,
    Valid,
    UnknownCommand,
    InvalidCoords,
    InvalidSurface,
    InvalidUnitType,
    InvalidUnitName,
    AlreadyHaveUnitOnNode,
    AlreadyHaveUnitWithThisName,
    NoUnitOnNode,
    NoUnitWithThisName
}