namespace Hunting.BL.Enum;

public static class DirectionExtensions
{
    public static Direction LeftSide(this Direction direction)
    {
        return direction switch
        {
            Direction.Left => Direction.Bot,
            Direction.Top => Direction.Left,
            Direction.Right => Direction.Top,
            Direction.Bot => Direction.Right,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
    
    public static Direction RightSide(this Direction direction)
    {
        return direction switch
        {
            Direction.Left => Direction.Top,
            Direction.Top => Direction.Right,
            Direction.Right => Direction.Bot,
            Direction.Bot => Direction.Left,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
    
    public static Direction ReverseSide(this Direction direction)
    {
        return direction switch
        {
            Direction.Left => Direction.Right,
            Direction.Top => Direction.Bot,
            Direction.Right => Direction.Left,
            Direction.Bot => Direction.Top,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}