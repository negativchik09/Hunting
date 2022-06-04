using Hunting.BL.Abstractions;

namespace Hunting.BL.Special;

public record ParsingResult(bool Valid, ICommand? Command = null, IContract? Contract = null)
{
    public ICommand? Command { get; set; } = Command;
    public IContract? Contract { get; set; } = Contract;
    public bool Valid { get; set; } = Valid;
}