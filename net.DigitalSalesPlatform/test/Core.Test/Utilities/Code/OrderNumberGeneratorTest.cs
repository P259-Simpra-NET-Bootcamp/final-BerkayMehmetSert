using Core.Utilities.Code;
using Xunit;

namespace Core.Test.Utilities.Code;

public class OrderNumberGeneratorTest
{
    [Fact]
    public void GenerateShouldReturnOrderNumberWithCorrectLength()
    {
        var orderNumber = OrderNumberGenerator.GenerateOrderNumber();
        Assert.Equal(9, orderNumber.ToString().Length);
    }
    
    [Fact]
    public void GenerateShouldReturnOrderNumberWithCorrectCharacters()
    {
        var orderNumber = OrderNumberGenerator.GenerateOrderNumber();
        Assert.Matches("^[0-9]*$", orderNumber.ToString());
    }
}