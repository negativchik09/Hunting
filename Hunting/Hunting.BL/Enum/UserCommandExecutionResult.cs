namespace Hunting.BL.Enum;

public enum UserCommandExecutionResult
{
    Success,
    UnknownCommand,
    InvalidCoords,
    InvalidSurface,
    InvalidUnitType,
    InvalidUnitName,
    AlreadyHaveUnitOnNode,
    AlreadyHaveUnitWithThisName
}