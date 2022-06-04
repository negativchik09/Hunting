using Hunting.BL.Abstractions;

namespace Hunting.BL.Commands.Contracts;

public record ChangeSurfaceContract(int X, int Y, string SurfaceType) : IContract;
