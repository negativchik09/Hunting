namespace Hunting.BL.Commands.Contracts;

public record RemoveUnitContract
{
    public RemoveUnitContract(int X, int Y)
    {
        this.X = X;
        this.Y = Y;
        this.UnitName = null;
        RemoveUnitContractType = RemoveUnitContractType.ByCoordinates;
    }
    
    public RemoveUnitContract(string UnitName)
    {
        this.X = -1;
        this.Y = -1;
        this.UnitName = UnitName;
        RemoveUnitContractType = RemoveUnitContractType.ByName;
    }

    public int X { get; init; }
    public int Y { get; init; }
    public string? UnitName { get; init; }
    public RemoveUnitContractType RemoveUnitContractType { get; init; }
}