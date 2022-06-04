namespace Hunting.BL.Matrix;

public class Meat
{
    private readonly int _baseHungerRegen;
    public int TurnsBeforeDispose { get; set; }
    public int HungerRegen
    {
        get => _baseHungerRegen + TurnsBeforeDispose;

        init => _baseHungerRegen = value;
    }
    public int Amount { get; set; }

    public int Eat()
    {
        if (TurnsBeforeDispose == 0)
        {
            return 0;
        }
        if (Amount <= 0)
        {
            return 0;
        }

        Amount -= 1;
        return HungerRegen;
    }
}