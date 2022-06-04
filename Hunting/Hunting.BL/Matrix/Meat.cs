namespace Hunting.BL.Matrix;

public class Meat
{
    private readonly double _baseHungerRegen;
    public int TurnsBeforeDispose { get; set; }

    public double HungerRegen
    {
        get => _baseHungerRegen + TurnsBeforeDispose;

        init => _baseHungerRegen = value;
    }

    public int Amount { get; set; }
}