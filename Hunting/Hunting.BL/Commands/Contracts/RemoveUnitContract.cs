using Hunting.BL.Abstractions;

namespace Hunting.BL.Commands.Contracts;

public record RemoveUnitContract : IContract
{
    public RemoveUnitContract(int x, int y)
    {
        ByCoords = new RemoveUnitByCoordsContract(x, y);
        ByName = null;
        RemoveUnitContractType = RemoveUnitContractType.ByCoordinates;
    }
    
    public RemoveUnitContract(string unitName)
    {
        ByName = new RemoveUnitByNameContract(unitName);
        ByCoords = null;
        RemoveUnitContractType = RemoveUnitContractType.ByName;
    }
    public RemoveUnitByCoordsContract ByCoords { get; init; }
    public RemoveUnitByNameContract ByName { get; init; }
    public RemoveUnitContractType RemoveUnitContractType { get; init; }
}