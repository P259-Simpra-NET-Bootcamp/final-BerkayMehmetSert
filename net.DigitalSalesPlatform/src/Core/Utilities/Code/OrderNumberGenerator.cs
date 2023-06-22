namespace Core.Utilities.Code;

public static class OrderNumberGenerator
{
    private static readonly Random Random = new Random();
    private const int MaxLength = 9;
    public static int GenerateOrderNumber()
    {
        var orderNumber = Random.Next(0, (int)Math.Pow(10, MaxLength));
        var orderNumberString = orderNumber.ToString().PadLeft(MaxLength, '0');
        return int.Parse(orderNumberString);
    }
    
}